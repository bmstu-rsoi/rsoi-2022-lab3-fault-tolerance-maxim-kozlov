using Serilog;

namespace FlightBooking.TicketService.Extensions;

internal static class HostBuilderExtensions
{
    internal static IHostBuilder SetupSerilog(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((hostingContext, configuration) =>
        {
            configuration.ReadFrom.Configuration(hostingContext.Configuration);
        });
    }
}