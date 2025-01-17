﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NodaTime;
using OpenTelemetry.Trace;
using Serilog;
using System.Configuration;
using System.Reflection;
using TrafficCourts.Citizen.Service.Configuration;
using TrafficCourts.Citizen.Service.Services;
using TrafficCourts.Citizen.Service.Validators;
using TrafficCourts.Citizen.Service.Services.Impl;
using TrafficCourts.Common;
using TrafficCourts.Common.Configuration;
using TrafficCourts.Messaging;
using TicketStorageType = TrafficCourts.Citizen.Service.Configuration.TicketStorageType;
using FluentValidation.AspNetCore;
using TrafficCourts.Common.Features.FilePersistence;

namespace TrafficCourts.Citizen.Service;

public static class Startup
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
    /// <exception cref="ConfigurationErrorsException"></exception>
    public static void ConfigureApplication(this WebApplicationBuilder builder, Serilog.ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // this assembly, used in a couple locations below for registering things
        Assembly assembly = Assembly.GetExecutingAssembly();

        builder.AddSerilog();
        builder.AddOpenTelemetry(Diagnostics.Source, logger, options =>
        {
            // add MassTransit source
            options
            .AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName)
            .AddRedisInstrumentation();
        });

        // Redis
        builder.AddRedis();

        // configure application 
        builder.Services.UseConfigurationValidation();
        builder.UseTicketSearch(logger);

        builder.Services.AddFilePersistence(builder.Configuration);

        // Form Recognizer
        builder.Services.ConfigureValidatableSetting<FormRecognizerOptions>(builder.Configuration.GetSection(FormRecognizerOptions.Section));
        builder.Services.AddTransient<IFormRecognizerService, FormRecognizerService>();
        builder.Services.AddTransient<IFormRecognizerValidator, FormRecognizerValidator>();

        builder.Services.AddSingleton<ILookupService, RedisLookupService>();
        builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();

        // MassTransit
        builder.Services.AddMassTransit(Diagnostics.Source.Name, builder.Configuration, logger);

        // add MediatR handlers in this program
        builder.Services.AddMediatR(assembly);

        // use lowercase routes
        builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        builder.Services.AddTransient<IConfigureOptions<JsonOptions>, ConfigureJsonOptions>();

        builder.Services.AddSingleton<IClock>(SystemClock.Instance);

        builder.Services.AddRecyclableMemoryStreams();
        
        builder.Services.AddAutoMapper(assembly); // Registering and Initializing AutoMapper
        
        builder.Services.AddFluentValidation(configure =>
        {
            // Finds and registers all the fluent validators in the assembly
            configure.RegisterValidatorsFromAssembly(assembly);
        });

        // Add services to the container.
        builder.Services
            .AddControllers(options => options.UseDateOnlyTimeOnlyStringConverters())
            .AddJsonOptions(options => options.UseDateOnlyTimeOnlyStringConverters());

        AddSwagger(builder, assembly, logger);
    }

    /// <summary>
    /// Adds swagger if enabled bu configuration
    /// </summary>
    private static void AddSwagger(WebApplicationBuilder builder, Assembly assembly, Serilog.ILogger logger)
    {
        var swagger = SwaggerConfiguration.Get(builder.Configuration);

        if (swagger.Enabled)
        {
            logger.Information("Swagger is enabled");

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "Traffic Court Online Citizen Api",
                    Description = "An API for creating violation ticket disputes",
                });

                options.UseDateOnlyTimeOnlyStringConverters();

                var xmlFilename = $"{assembly.GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }
    }
}
