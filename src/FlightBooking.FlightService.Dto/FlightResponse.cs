using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace FlightBooking.FlightService.Dto;

public class FlightResponse
{
    /// <summary>
    /// Номер полета
    /// </summary>
    [JsonProperty("flightNumber")]
    public string FlightNumber { get; set; }
    
    /// <summary>
    /// Страна и аэропорт отправления
    /// </summary>
    [JsonProperty("fromAirport")]
    public string FromAirport { get; set; }
    
    /// <summary>
    /// Страна и аэропорт прибытия
    /// </summary>
    [JsonProperty("toAirport")]
    public string ToAirport { get; set; }
    
    /// <summary>
    /// Дата и время вылета
    /// </summary>
    [JsonProperty("date")]
    public DateTime Date { get; set; }
    
    /// <summary>
    /// Стоимость
    /// </summary>
    [JsonProperty("price")]
    public double Price { get; set; }
}