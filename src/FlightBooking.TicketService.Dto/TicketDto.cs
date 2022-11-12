using Newtonsoft.Json;

namespace FlightBooking.TicketService.Dto;

public class TicketDto
{
    /// <summary>
    /// UUID билета
    /// </summary>
    [JsonProperty("ticketUid")]
    public Guid TicketUid { get; set; }
    
    /// <summary>
    /// Номер полета
    /// </summary>
    [JsonProperty("flightNumber")]
    public string FlightNumber { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    [JsonProperty("username")]
    public string Username { get; set; }
    
    /// <summary>
    /// Стоимость
    /// </summary>
    [JsonProperty("price")]
    public int Price { get; set; }
    
    /// <summary>
    /// Статус билета
    /// </summary>
    [JsonProperty("status")]
    public TicketStatusDto Status { get; set; }
}