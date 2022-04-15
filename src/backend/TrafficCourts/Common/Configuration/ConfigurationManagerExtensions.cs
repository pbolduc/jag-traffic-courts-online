using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Diagnostics;

namespace TrafficCourts.Common.Configuration;

public static class ConfigurationManagerExtensions
{
    /// <summary>
    /// Adds the specified <see cref="ConfigurationProvider"/> as a configuration source.
    /// </summary>
    /// <typeparam name="TConfigurationProvider">Type of the configuration provider</typeparam>
    /// <param name="configurationManager"></param>
    public static void Add<TConfigurationProvider>(this ConfigurationManager configurationManager) 
        where TConfigurationProvider : TrafficCourtsConfigurationProvider, new()
    {
        ArgumentNullException.ThrowIfNull(configurationManager);
        Debug.Assert(configurationManager is IConfigurationBuilder);

        ((IConfigurationBuilder)configurationManager).Add(new TrafficCourtsConfigurationSource<TConfigurationProvider>());
    }

    /// <summary>
    /// Adds IConnectionMultiplexer as a singleton using a connection string found in key &quot;Redis:ConnectionString&quot;.
    /// </summary>
    /// <param name="builder"></param>
    public static void AddRedis(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(ConnectionMultiplexerFactory);
    }

    /// <summary>
    /// Factory method for creating the Redis <see cref="IConnectionMultiplexer"/> instance.
    /// </summary>
    /// <param name="serviceProvider"></param>
    private static IConnectionMultiplexer ConnectionMultiplexerFactory(IServiceProvider serviceProvider)
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        string connectionString = configuration.GetValue<string>("Redis:ConnectionString");

        // TODO: Redis:ConnectionString is missing, connectionString may be null

        ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
        return connectionMultiplexer;
    }

}
