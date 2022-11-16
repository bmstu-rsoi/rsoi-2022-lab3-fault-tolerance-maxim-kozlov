using System.Runtime.Serialization;

namespace FlightBooking.TicketService.Dto;

public enum TicketStatusDto
{ 
    [EnumMember(Value = "PAID")]
    Paid = 0,
    
    [EnumMember(Value = "CANCELED")]
    Canceled = 1
}