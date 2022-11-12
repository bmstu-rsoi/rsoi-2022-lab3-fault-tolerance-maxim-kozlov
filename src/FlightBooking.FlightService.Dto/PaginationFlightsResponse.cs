using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace FlightBooking.FlightService.Dto;

public class PaginationFlightsResponse
{
    /// <summary>
    /// Номер страницы
    /// </summary>
    [JsonProperty("page")]
    public int Page { get; set; }
    
    /// <summary>
    /// Количество элементов на странице
    /// </summary>
    [JsonProperty("pageSize")]
    public int PageSize { get; set; }
    
    /// <summary>
    /// Общее количество элементов
    /// </summary>
    [JsonProperty("totalElements")]
    public int TotalElements { get; set; }
    
    /// <summary>
    /// Массив рейсов
    /// </summary>
    [JsonProperty("items")]
    public FlightResponse[] Flights { get; set; }
}