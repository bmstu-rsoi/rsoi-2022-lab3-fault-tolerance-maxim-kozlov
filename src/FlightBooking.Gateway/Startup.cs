using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using FlightBooking.Gateway.Domain;
using FlightBooking.Gateway.Extensions;
using FlightBooking.Gateway.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Polly;
using Polly.Extensions.Http;

namespace FlightBooking.Gateway;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddUrlGroup(new Uri(Configuration["FlightsService:Host"] + "/manage/health"), name: "flights-service-check", failureStatus: HealthStatus.Degraded)
            .AddUrlGroup(new Uri(Configuration["TicketService:Host"] + "/manage/health"), name: "ticket-service-check", failureStatus: HealthStatus.Degraded)
            .AddUrlGroup(new Uri(Configuration["PrivilegeService:Host"] + "/manage/health"), name: "bonus-service-check", failureStatus: HealthStatus.Degraded);
        
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo {Title = "FlightBooking.Gateway", Version = "v1"});
            
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            if (File.Exists(xmlPath))
                c.IncludeXmlComments(xmlPath);
        });

        // register http services
        services.Configure<FlightsSettings>(Configuration.GetSection("FlightsService"));
        services.AddHttpClient<IFlightsRepository, FlightsRepository>()
            // .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());
        
        services.Configure<TicketsSettings>(Configuration.GetSection("TicketService"));
        services.AddHttpClient<ITicketsRepository, TicketsRepository>()
            // .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());
        
        services.Configure<PrivilegeSettings>(Configuration.GetSection("PrivilegeService"));
        services.AddHttpClient<IPrivilegeRepository, PrivilegeRepository>()
            // .AddPolicyHandler(GetWaitAndRetryForeverPolicy())
            // .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());
        
        services.AddScoped<ITicketsService, TicketsService>();

        services.AddMassTransit(Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlightBooking.Gateway v1"));
        
        // app.UseHttpsRedirection();
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/manage/health");
            endpoints.MapHealthChecks("/manage/health/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });
        });
    }
    
    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(15, TimeSpan.FromSeconds(10));
    }
    
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}