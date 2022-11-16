using AutoMapper;
using FlightBooking.FlightService.Dto;

namespace FlightBooking.FlightService.Mapper;

public class FlightsProfile : Profile
{
    public FlightsProfile()
    {
        CreateMap<Database.Entities.Flight, FlightResponse>()
            .ForMember(x => x.FromAirport, s => s.MapFrom(p => $"{p.FromAirport.City} {p.FromAirport.Name}"))
            .ForMember(x => x.ToAirport, s => s.MapFrom(p => $"{p.ToAirport.City} {p.ToAirport.Name}"))
            .ForMember(x => x.Date, s => s.MapFrom(p => p.Datetime))
            .ForMember(x => x.Price, s => s.MapFrom(p => p.Price))
            .ForMember(x => x.FlightNumber, s => s.MapFrom(p => p.FlightNumber));
    }
}