using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using FlightBooking.Gateway.Exceptions;
using FlightBooking.Gateway.Extensions;
using FlightBooking.TicketService.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly.CircuitBreaker;

namespace FlightBooking.Gateway.Repositories;

public class TicketsSettings 
{
    public Uri Host { get; set; }
}

public class TicketsRepository : ITicketsRepository
{
    private readonly ILogger<TicketsRepository> _logger;
    private readonly HttpClient _client;

    public TicketsRepository(IOptions<TicketsSettings> settings, HttpClient httpClient, ILogger<TicketsRepository> logger)
    {
        _client = httpClient;
        _client.BaseAddress = settings.Value.Host;
        _logger = logger;
    }
    
    public async Task<TicketDto[]> GetAllAsync(string username)
    {
        try
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["username"] = username;
        
            var response = await _client.GetAsync($"/api/v1/tickets/?{query}");
            if (!response.IsSuccessStatusCode)
                _logger.LogWarning("Failed get tickets {statusCode}, {descriprion}", response.StatusCode, response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();
        
            return await response.Content.ReadAsJsonAsync<TicketDto[]>() ?? throw new InvalidOperationException();
        }
        catch (Exception ex) when(ex is HttpRequestException{ StatusCode: >= (HttpStatusCode)500 or null } or BrokenCircuitException)
        {
            throw new ServiceUnavailableException("Failed get flights", ex, serviceName: "Ticket Service");
        }
    }

    public async Task<TicketDto> GetAsync(string username, Guid ticketId)
    {
        try
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["username"] = username;
        
            var response = await _client.GetAsync($"/api/v1/tickets/{ticketId}/?{query}");
            if (!response.IsSuccessStatusCode)
                _logger.LogWarning("Failed get ticket {statusCode}, {descriprion}", response.StatusCode, response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<TicketDto>() ?? throw new InvalidOperationException();
        }
        catch (Exception ex) when(ex is HttpRequestException{ StatusCode: >= (HttpStatusCode)500 or null } or BrokenCircuitException)
        {
            throw new ServiceUnavailableException("Failed get flights", ex, serviceName: "Ticket Service");
        }
    }

    public async Task<TicketDto> CreateAsync(TicketDto ticketDto)
    {
        try
        {
            var response = await _client.PostAsJsonAsync("/api/v1/tickets/", ticketDto);
            if (!response.IsSuccessStatusCode)
                _logger.LogWarning("Failed get ticket {statusCode}, {descriprion}", response.StatusCode, response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<TicketDto>() ?? throw new InvalidOperationException();
        }
        catch (Exception ex) when(ex is HttpRequestException{ StatusCode: >= (HttpStatusCode)500 or null } or BrokenCircuitException)
        {
            throw new ServiceUnavailableException("Failed get flights", ex, serviceName: "Ticket Service");
        }
    }
    
    public async Task DeleteAsync(string username, Guid ticketId)
    {
        try
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["username"] = username;
        
            var response = await _client.DeleteAsync($"/api/v1/tickets/{ticketId}/?{query}");
            if (!response.IsSuccessStatusCode)
                _logger.LogWarning("Failed delete ticket {statusCode}, {descriprion}", response.StatusCode, response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex) when(ex is HttpRequestException{ StatusCode: >= (HttpStatusCode)500 or null } or BrokenCircuitException)
        {
            throw new ServiceUnavailableException("Failed get flights", ex, serviceName: "Ticket Service");
        }
    }
}