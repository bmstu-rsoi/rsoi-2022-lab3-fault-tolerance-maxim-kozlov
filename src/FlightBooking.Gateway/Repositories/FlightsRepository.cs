using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;
using FlightBooking.FlightService.Dto;
using FlightBooking.Gateway.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FlightBooking.Gateway.Repositories;

public class FlightsSettings 
{
    public Uri Host { get; set; }
}

public class FlightsRepository : IFlightsRepository
{
    private readonly ILogger<FlightsRepository> _logger;
    private readonly HttpClient _client;

    public FlightsRepository(IOptions<FlightsSettings> settings, HttpClient httpClient, ILogger<FlightsRepository> logger)
    {
        _client = httpClient;
        _client.BaseAddress = settings.Value.Host;
        _logger = logger;
    }
    
    public async Task<PaginationFlightsResponse> GetAllAsync(int page, int size)
    {
        var response = await _client.GetAsync($"/api/v1/flights/?page={page}&size={size}");
        if (!response.IsSuccessStatusCode)
            _logger.LogWarning("Failed get flights {statusCode}, {descriprion}", response.StatusCode, response.Content.ReadAsStringAsync());
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsJsonAsync<PaginationFlightsResponse>() ?? throw new InvalidOperationException();
    }

    public async Task<FlightResponse> GetByNumberAsync(string flightNumber)
    {
        var response = await _client.GetAsync($"/api/v1/flights/{HttpUtility.UrlEncode(flightNumber)}");
        if (!response.IsSuccessStatusCode)
            _logger.LogWarning("Failed get flights {statusCode}, {descriprion}", response.StatusCode, response.Content.ReadAsStringAsync());
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsJsonAsync<FlightResponse>() ?? throw new InvalidOperationException();
    }
}