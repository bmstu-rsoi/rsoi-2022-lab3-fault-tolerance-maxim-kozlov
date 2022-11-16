using Microsoft.Extensions.Hosting;
using Serilog;

namespace FlightBooking.Gateway.Extensions;

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