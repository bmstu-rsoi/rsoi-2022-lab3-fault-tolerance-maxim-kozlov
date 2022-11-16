using System.Runtime.Serialization;

namespace FlightBooking.BonusService.Dto;

public enum PrivilegeStatusDto
{
    [EnumMember(Value = "BRONZE")]
    Bronze = 0,
    
    [EnumMember(Value = "SILVER")]
    Silver = 1,
    
    [EnumMember(Value = "GOLD")]
    Gold = 2
}