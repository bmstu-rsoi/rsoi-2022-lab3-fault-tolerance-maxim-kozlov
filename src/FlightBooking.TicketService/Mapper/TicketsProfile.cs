using System.Runtime.Serialization;
using AutoMapper;
using FlightBooking.TicketService.Dto;

namespace FlightBooking.TicketService.Mapper;

public class TicketsProfile : Profile
{
    public TicketsProfile()
    {
        CreateMap<Database.Entities.Ticket, TicketDto>()
            .ForMember(x => x.TicketUid, s => s.MapFrom(p => p.TicketUid));

        CreateMap<TicketDto, Database.Entities.Ticket>()
            .ForMember(x => x.TicketUid, s => s.MapFrom(p => p.TicketUid))
            .ForMember(x => x.Status, s => s.MapFrom(p => GetValue(p.Status)));
    }
    
    private static string GetValue<T>(T type) where T: Enum
    {
        var enumType = typeof(T);
        var name = Enum.GetName(enumType, type);
        var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
        return enumMemberAttribute.Value;        
    }
}