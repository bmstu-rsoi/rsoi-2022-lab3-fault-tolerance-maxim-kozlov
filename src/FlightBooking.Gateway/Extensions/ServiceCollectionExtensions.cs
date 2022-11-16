using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FlightBooking.Gateway.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<RabbitMqTransportOptions>().BindConfiguration(nameof(RabbitMqTransportOptions));

        services.AddMassTransit(cfg =>
        {
            cfg.ConfigureHealthCheckOptions(x =>
            {
                x.FailureStatus = HealthStatus.Degraded;
            });
            cfg.UsingRabbitMq((context, config) =>
            {
                config.UseBsonSerializer();
                config.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}