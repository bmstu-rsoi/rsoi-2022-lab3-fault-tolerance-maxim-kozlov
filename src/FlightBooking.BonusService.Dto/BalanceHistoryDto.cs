using Newtonsoft.Json;

namespace FlightBooking.BonusService.Dto;

public class BalanceHistoryDto
{
    /// <summary>
    /// Дата и время операции
    /// </summary>
    [JsonProperty("date")]
    public DateTime DateTime { get; set; }
    
    /// <summary>
    /// Изменение баланса
    /// </summary>
    [JsonProperty("balanceDiff")]
    public int BalanceDiff { get; set; }
    
    /// <summary>
    /// UUID билета по которому была операция с бонусами
    /// </summary>
    [JsonProperty("ticketUid")]
    public Guid TicketUid { get; set; }
    
    /// <summary>
    /// Тип операции
    /// </summary>
    [JsonProperty("operationType")]
    public OperationTypeDto OperationType { get; set; }
}