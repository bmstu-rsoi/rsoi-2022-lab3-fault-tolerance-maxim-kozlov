using System;
using System.Threading.Tasks;
using FlightBooking.BonusService.Dto;

namespace FlightBooking.Gateway.Repositories;

public interface IPrivilegeRepository
{
    Task<PrivilegeDto> GetAsync(string username, bool needHistory);
    
    Task<BalanceHistoryDto> CreateAsync(string username, TicketPurchaseRequest request);
    
    Task DeleteAsync(string username, Guid ticketId);
}