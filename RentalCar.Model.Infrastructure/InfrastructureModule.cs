using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RentalCar.Model.Core.Repositories;
using RentalCar.Model.Core.Services;
using RentalCar.Model.Infrastructure.MessageBus;
using RentalCar.Model.Infrastructure.Prometheus;
using RentalCar.Model.Infrastructure.Repositories;
using RentalCar.Model.Infrastructure.Services;

namespace RentalCar.Model.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) 
    {
        services
            .AddServices()
            .AddOpenTelemetryConfig()
            ;
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services) 
    {
        services.AddSingleton<ILoggerService, LoggerService>();
        services.AddSingleton<IRabbitMqService, RabbitMqService>();
        services.AddSingleton<IPrometheusService, PrometheusService>();
        services.AddSingleton<IModelService, ModelService>();

        //services.AddSingleton<IRedisRepository, RedisRepository>();
        services.AddScoped<IModelRepository, ModelRepository>();

        return services;
    }
    
    private static IServiceCollection AddOpenTelemetryConfig(this IServiceCollection services)
    {
        const string serviceName = "RentalCar Model";
        const string serviceVersion = "v1";
        
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing => tracing
                .SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService(serviceName: serviceName, serviceVersion:serviceVersion))
                .AddAspNetCoreInstrumentation()
                .AddOtlpExporter()
                .AddConsoleExporter())
            .WithMetrics(metrics => metrics
                .AddConsoleExporter()
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddPrometheusExporter()
            );

        return services;
    }

}