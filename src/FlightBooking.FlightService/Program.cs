using FlightBooking.FlightService.Database;
using FlightBooking.FlightService.Database.Entities;
using FlightBooking.FlightService.Extensions;
using Serilog;

namespace FlightBooking.FlightService;

public static class Program
{
    private const string _appsettingsFilename = "appsettings.json";
    
    public static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(_appsettingsFilename, optional: false)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();
        
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateBootstrapLogger();
        
        try
        {
            var host = CreateHostBuilder(args, configuration).Build();
            await host.RunAsync();
        }
        catch (Exception e)
        {
            Log.Error(e, "Stopped program because of exception!");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration)
    {
        return Host.CreateDefaultBuilder(args)
            .SetupSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseUrls("http://*:" + configuration.GetValue<int>("PORT"));
            });
    }
}