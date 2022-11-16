using System.Threading.Tasks;
using FlightBooking.FlightService.Dto;

namespace FlightBooking.Gateway.Repositories;

public interface IFlightsRepository
{
    Task<PaginationFlightsResponse> GetAllAsync(int page, int size);
    
    Task<FlightResponse> GetByNumberAsync(string flightNumber);
}