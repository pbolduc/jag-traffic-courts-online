using MassTransit;
using MediatR;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using System.Reflection;
using System.Text.Json.Serialization;
using TrafficCourts.Common.Configuration;
using TrafficCourts.Messaging;
using TrafficCourts.Staff.Service.Authentication;
using TrafficCourts.Staff.Service.Configuration;
using TrafficCourts.Staff.Service.Logging;
using TrafficCourts.Staff.Service.Services;

var builder = WebApplication.CreateBuilder(args);
var logger = GetLogger(builder);

// Add services to the container.

AddOpenTelemetry(builder, logger);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) => {
    loggerConfiguration
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder().WithDefaultDestructurers());
});

builder.Services.AddMassTransit(builder.Configuration, logger);

// Render enums as strings rather than ints
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

Authentication.Initialize(builder.Services, builder.Configuration);

// Add DisputeService
builder.Services.ConfigureValidatableSetting<OracleDataApiConfiguration>(builder.Configuration.GetRequiredSection(OracleDataApiConfiguration.Section));
builder.Services.AddSingleton<IDisputeService, DisputeService>();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

var swagger = SwaggerConfiguration.Get(builder.Configuration);

if (swagger.Enabled)
{
    logger.Information("Swagger is enabled");
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c => 
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Version = "v1",
            Title = "VTC Staff API",
            Description = "Violation Ticket Centre Staff API",
        });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement 
        {
            {
                new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                Array.Empty<string>()
            } 
        });

        c.EnableAnnotations();

        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (swagger.Enabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
    .RequireAuthorization(); // This will set a default policy that says a user has to be authenticated

app.Run();

static Serilog.ILogger GetLogger(WebApplicationBuilder builder)
{
    var configuration = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.Console();

    if (builder.Environment.IsDevelopment())
    {
        configuration.WriteTo.Debug();
    }

    return configuration.CreateLogger();
}

static void AddOpenTelemetry(WebApplicationBuilder builder, Serilog.ILogger logger)
{
    string? endpoint = builder.Configuration["OTEL_EXPORTER_JAEGER_ENDPOINT"];

    if (string.IsNullOrEmpty(endpoint))
    {
        logger.Information("Jaeger endpoint is not configured, no telemetry will be collected.");
        return;
    }

    var resourceBuilder = ResourceBuilder.CreateDefault().AddService(Diagnostics.Source.Name, serviceInstanceId: Environment.MachineName);

    builder.Services.AddOpenTelemetryTracing(options =>
    {
        options
            .SetResourceBuilder(resourceBuilder)
            .AddHttpClientInstrumentation(options =>
            {
                    // do not trace calls to splunk
                    options.Filter = (message) => message.RequestUri?.Host != "hec.monitoring.ag.gov.bc.ca";

            })
            .AddAspNetCoreInstrumentation()
            .AddSource(Diagnostics.Source.Name)
            .AddJaegerExporter();
    });
}
