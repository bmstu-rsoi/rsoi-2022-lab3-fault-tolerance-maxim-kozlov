using System;
using System.Threading.Tasks;
using FlightBooking.Gateway.Dto.Tickets;
using FlightBooking.TicketService.Dto;

namespace FlightBooking.Gateway.Repositories;

public interface ITicketsRepository
{
    Task<TicketDto[]> GetAllAsync(string username);
    
    Task<TicketDto> GetAsync(string username, Guid ticketId);
    
    Task<TicketDto> CreateAsync(TicketDto ticketDto);
    
    Task DeleteAsync(string username, Guid ticketId);
}