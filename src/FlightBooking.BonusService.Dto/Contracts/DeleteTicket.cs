namespace FlightBooking.BonusService.Dto.Contracts;

public interface DeleteTicket
{
    Guid TicketUid { get; set; }
    
    string Username { get; set; }
}