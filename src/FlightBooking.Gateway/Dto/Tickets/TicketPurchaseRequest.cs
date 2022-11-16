using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace FlightBooking.Gateway.Dto.Tickets;

public class TicketPurchaseRequest
{
    /// <summary>
    /// Номер полета
    /// </summary>
    [JsonProperty("flightNumber")]
    public string FlightNumber { get; set; }
    
    /// <summary>
    /// Стоимость
    /// </summary>
    [JsonProperty("price")]
    [Range(0, int.MaxValue)]
    public int Price { get; set; }
    
    /// <summary>
    /// Выполнить списание бонусных баллов в учет покупки билета
    /// </summary>
    [JsonProperty("paidFromBalance")]
    public bool PaidFromBalance { get; set; }
}