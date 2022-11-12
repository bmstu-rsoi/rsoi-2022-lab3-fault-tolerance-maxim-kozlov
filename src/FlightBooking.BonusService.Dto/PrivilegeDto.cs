using Newtonsoft.Json;

namespace FlightBooking.BonusService.Dto;

public class PrivilegeDto
{
    /// <summary>
    /// Баланс бонусного счета
    /// </summary>
    [JsonProperty("balance")]
    public int Balance { get; set; }
    
    /// <summary>
    /// Статус в бонусной программе
    /// </summary>
    [JsonProperty("status")]
    public PrivilegeStatusDto Status { get; set; }
    
    /// <summary>
    /// История изменения баланса
    /// </summary>
    [JsonProperty("history")]
    public BalanceHistoryDto[]? History { get; set; }
}