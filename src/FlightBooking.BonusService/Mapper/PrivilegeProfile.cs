using AutoMapper;
using FlightBooking.BonusService.Dto;
using FlightBooking.BonusService.Extensions;

namespace FlightBooking.BonusService.Mapper;

public class PrivilegeProfile : Profile
{
    public PrivilegeProfile()
    {
        CreateMap<Database.Entities.PrivilegeHistory, Database.Entities.PrivilegeHistory>()
            .ForMember(x => x.Id, opt => opt.Ignore());
        
        CreateMap<Database.Entities.Privilege, PrivilegeDto>()
            .ForMember(x => x.History, opt => opt.MapFrom(p => p.PrivilegeHistories));
        
        CreateMap<PrivilegeDto, Database.Entities.Privilege>()
            .ForMember(x => x.Status, opt => opt.MapFrom(p => p.Status.GetValue()));
        
        CreateMap<Database.Entities.PrivilegeHistory, BalanceHistoryDto>();

        CreateMap<BalanceHistoryDto, Database.Entities.PrivilegeHistory>()
            .ForMember(x => x.OperationType, opt => opt.MapFrom(p => p.OperationType.GetValue()));
        
        CreateMap<TicketPurchaseRequest, Database.Entities.PrivilegeHistory>();
    }
}