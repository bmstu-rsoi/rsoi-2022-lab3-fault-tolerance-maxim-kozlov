using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using FlightBooking.BonusService.Dto;
using FlightBooking.Gateway.Dto.Tickets;
using FlightBooking.Gateway.Repositories;
using FlightBooking.TicketService.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;
using TicketPurchaseRequest = FlightBooking.Gateway.Dto.Tickets.TicketPurchaseRequest;

namespace FlightBooking.Gateway.Domain;

public class TicketsService : ITicketsService
{
    private readonly ILogger<TicketsService> _logger;
    private readonly ITicketsRepository _ticketsRepository;
    private readonly IFlightsRepository _flightsRepository;
    private readonly IPrivilegeRepository _privilegeRepository;
    private readonly IMapper _mapper;
    
    public TicketsService(ILogger<TicketsService> logger, ITicketsRepository ticketsRepository, IFlightsRepository flightsRepository, IMapper mapper, IPrivilegeRepository privilegeRepository)
    {
        _logger = logger;
        _ticketsRepository = ticketsRepository;
        _flightsRepository = flightsRepository;
        _mapper = mapper;
        _privilegeRepository = privilegeRepository;
    }
    
    public async Task<TicketResponse> GetAsync(string username, Guid ticketUid)
    {
        var ticket = await _ticketsRepository.GetAsync(username, ticketUid);
        var response = _mapper.Map<TicketResponse>(ticket);

        await AddFlightInfoAsync(ticket.FlightNumber, response);

        return response;
    }
    
    public async Task<List<TicketResponse>> GetAllAsync(string username)
    {
        var tickets =  await _ticketsRepository.GetAllAsync(username);
        var response = new List<TicketResponse>(tickets.Length);
            
        foreach (var ticket in tickets)
        {
            var res = _mapper.Map<TicketResponse>(ticket);
            
            await AddFlightInfoAsync(ticket.FlightNumber, res);
            
            response.Add(res);
        }

        return response;
    }

    public async Task<TicketPurchaseResponse> PurchaseAsync(string username, TicketPurchaseRequest request)
    {
        // по-хорошему транзация
        var response = new TicketPurchaseResponse();

        var newTicket = _mapper.Map<TicketPurchaseRequest, TicketDto>(request);
        newTicket.Status = TicketStatusDto.Paid;
        newTicket.Username = username;
        var createdTicket = await _ticketsRepository.CreateAsync(newTicket);
        
        // var privilege = await _privilegeRepository.GetAsync(username, needHistory: false);
        // var operation = new BalanceHistoryDto()
        // {
        //     TicketUid = createdTicket.TicketUid,
        //     DateTime = DateTime.Now
        // };
        // if (request.PaidFromBalance)
        // {
        //     if (request.Price > privilege.Balance)
        //         operation.BalanceDiff = -privilege.Balance;
        //     else
        //         operation.BalanceDiff = -request.Price;
        //     operation.OperationType = OperationTypeDto.DebitAccount;
        // }
        // else
        // {
        //     operation.BalanceDiff = (int)(request.Price * 0.1);
        //     operation.OperationType = OperationTypeDto.FilledByMoney;
        // }
        
        var bonusRequest = new FlightBooking.BonusService.Dto.TicketPurchaseRequest
        {
            TicketUid = createdTicket.TicketUid,
            Price = request.Price,
            PaidFromBalance = request.PaidFromBalance
        };
        
        var operation = await _privilegeRepository.CreateAsync(username, bonusRequest);
        if (request.PaidFromBalance)
            response.paidByBonuses = -operation.BalanceDiff;
        response.paidByMoney = request.Price - response.paidByBonuses;

        response.Privilege = await _privilegeRepository.GetAsync(username, needHistory: false);
        response.TicketId = createdTicket.TicketUid;
        
        await AddFlightInfoAsync(request.FlightNumber, response);

        return response;
    }

    public async Task DeleteAsync(string username, Guid ticketId)
    {
        await _ticketsRepository.DeleteAsync(username, ticketId);
        await _privilegeRepository.DeleteAsync(username, ticketId);
    }
    
    private async Task<T> AddFlightInfoAsync<T>(string flightNumber, T ticket)
    {
        try
        {
            var flight = await _flightsRepository.GetByNumberAsync(flightNumber);
            _mapper.Map(flight, ticket);
        }
        catch (BrokenCircuitException ex)
        {
            _logger.LogWarning(ex, "Flight service is inoperative, please try later on (BrokenCircuit)");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Error while getting flights info for {flightNumber}", flightNumber);
        }

        return ticket;
    }
}