using FlightBooking.BonusService.Consumers;
using MassTransit;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FlightBooking.BonusService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<RabbitMqTransportOptions>().BindConfiguration(nameof(RabbitMqTransportOptions));

        services.AddMassTransit(cfg =>
        {
            cfg.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(configuration.GetValue<string>("EndpointPrefix"), false));
            cfg.AddConsumer<DeleteTicketConsumer>();
         
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