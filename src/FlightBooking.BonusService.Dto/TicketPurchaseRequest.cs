using Newtonsoft.Json;

namespace FlightBooking.BonusService.Dto;

public class TicketPurchaseRequest
{
    /// <summary>
    /// UUID билета
    /// </summary>
    [JsonProperty("ticketUid")]
    public Guid TicketUid { get; set; }
    
    /// <summary>
    /// Стоимость
    /// </summary>
    [JsonProperty("price")]
    public int Price { get; set; }
    
    /// <summary>
    /// Выполнить списание бонусных баллов в учет покупки билета
    /// </summary>
    [JsonProperty("paidFromBalance")]
    public bool PaidFromBalance { get; set; }
}