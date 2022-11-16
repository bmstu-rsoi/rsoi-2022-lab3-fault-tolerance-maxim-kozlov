using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using FlightBooking.BonusService.Dto;
using FlightBooking.BonusService.Dto.Contracts;
using FlightBooking.Gateway.Exceptions;
using FlightBooking.Gateway.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly.CircuitBreaker;

namespace FlightBooking.Gateway.Repositories;

public class PrivilegeSettings 
{
    public Uri Host { get; set; }
}

public class PrivilegeRepository : IPrivilegeRepository
{
    private readonly ILogger<PrivilegeRepository> _logger;
    private readonly HttpClient _client;
    private readonly IServiceProvider _serviceProvider;
        
    public PrivilegeRepository(IOptions<PrivilegeSettings> settings, HttpClient httpClient, ILogger<PrivilegeRepository> logger, IServiceProvider serviceProvider)
    {
        _client = httpClient;
        _client.BaseAddress = settings.Value.Host;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
    
    public async Task<PrivilegeDto> GetAsync(string username, bool needHistory)
    {
        try
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["username"] = username;
            query["needHistory"] = needHistory.ToString();

            var response = await _client.GetAsync($"api/v1/privilege/?{query}");
            if (!response.IsSuccessStatusCode)
                _logger.LogWarning("Failed get tickets {statusCode}, {descriprion}", response.StatusCode, response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();
        
            return await response.Content.ReadAsJsonAsync<PrivilegeDto>() ?? throw new InvalidOperationException();
        }
        catch (Exception ex) when(ex is HttpRequestException{ StatusCode: >= (HttpStatusCode)500 or null } or BrokenCircuitException)
        {
            throw new ServiceUnavailableException("Failed get flights", ex, serviceName: "Bonus Service");
        }
    }

    public async Task<BalanceHistoryDto> CreateAsync(string username, TicketPurchaseRequest request)
    {
        try
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["username"] = username;

            var response = await _client.PostAsJsonAsync($"api/v1/history/?{query}", request);
            if (!response.IsSuccessStatusCode)
                _logger.LogWarning("Failed get tickets {statusCode}, {descriprion}", response.StatusCode, response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();
        
            return await response.Content.ReadAsJsonAsync<BalanceHistoryDto>() ?? throw new InvalidOperationException();
        }
        catch (Exception ex) when(ex is HttpRequestException{ StatusCode: >= (HttpStatusCode)500 or null } or BrokenCircuitException)
        {
            throw new ServiceUnavailableException("Failed get flights", ex, serviceName: "Bonus Service");
        }
    }
    
    public async Task DeleteAsync(string username, Guid ticketId)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var publisher = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        await publisher.Publish<DeleteTicket>(new
        {
            TicketUid = ticketId,
            Username = username
        });

        // try
        // {
        //     var query = HttpUtility.ParseQueryString(string.Empty);
        //     query["username"] = username;
        //
        //     var response = await _client.DeleteAsync($"/api/v1/history/{ticketId}/?{query}");
        //     if (!response.IsSuccessStatusCode)
        //         _logger.LogWarning("Failed delete ticket {statusCode}, {descriprion}", response.StatusCode, response.Content.ReadAsStringAsync());
        //     response.EnsureSuccessStatusCode();
        // }
        // catch (Exception ex) when(ex is HttpRequestException{ StatusCode: >= (HttpStatusCode)500 or null } or BrokenCircuitException)
        // {
        //     throw new ServiceUnavailableException("Failed get flights", ex, serviceName: "Bonus Service");
        // }
    }
}