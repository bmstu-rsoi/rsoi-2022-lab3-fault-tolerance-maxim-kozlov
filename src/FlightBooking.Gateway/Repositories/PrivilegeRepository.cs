using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using FlightBooking.BonusService.Dto;
using FlightBooking.Gateway.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FlightBooking.Gateway.Repositories;

public class PrivilegeSettings 
{
    public Uri Host { get; set; }
}

public class PrivilegeRepository : IPrivilegeRepository
{
    private readonly ILogger<PrivilegeRepository> _logger;
    private readonly HttpClient _client;

    public PrivilegeRepository(IOptions<PrivilegeSettings> settings, HttpClient httpClient, ILogger<PrivilegeRepository> logger)
    {
        _client = httpClient;
        _client.BaseAddress = settings.Value.Host;
        _logger = logger;
    }
    
    public async Task<PrivilegeDto> GetAsync(string username, bool needHistory)
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

    public async Task<BalanceHistoryDto> CreateAsync(string username, TicketPurchaseRequest request)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["username"] = username;

        var response = await _client.PostAsJsonAsync($"api/v1/history/?{query}", request);
        if (!response.IsSuccessStatusCode)
            _logger.LogWarning("Failed get tickets {statusCode}, {descriprion}", response.StatusCode, response.Content.ReadAsStringAsync());
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsJsonAsync<BalanceHistoryDto>() ?? throw new InvalidOperationException();
    }
    
    public async Task DeleteAsync(string username, Guid ticketId)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["username"] = username;
        
        var response = await _client.DeleteAsync($"/api/v1/history/{ticketId}/?{query}");
        if (!response.IsSuccessStatusCode)
            _logger.LogWarning("Failed delete ticket {statusCode}, {descriprion}", response.StatusCode, response.Content.ReadAsStringAsync());
        response.EnsureSuccessStatusCode();
    }
}