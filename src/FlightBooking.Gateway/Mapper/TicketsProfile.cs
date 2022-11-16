using AutoMapper;
using FlightBooking.FlightService.Dto;
using FlightBooking.Gateway.Dto.Tickets;
using FlightBooking.TicketService.Dto;

namespace FlightBooking.Gateway.Mapper;

public class TicketsProfile : Profile
{
    public TicketsProfile()
    {
        CreateMap<TicketDto, TicketResponse>()
            .ForMember(x => x.TicketId, s => s.MapFrom(p => p.TicketUid));;
        
        CreateMap<FlightResponse, TicketResponse>();
        CreateMap<FlightResponse, TicketPurchaseResponse>();
        
        CreateMap<TicketPurchaseRequest, TicketDto>();
    }
}