using FlightBooking.BonusService.Dto;
using FlightBooking.Gateway.Dto.Tickets;
using Newtonsoft.Json;

namespace FlightBooking.Gateway.Dto.Users;

public class UserInfoResponse
{
    /// <summary>
    /// Информация о билетах пользователя
    /// </summary>
    [JsonProperty("tickets")]
    public TicketResponse[] Tickets { get; set; }
    
    /// <summary>
    /// Баланс бонусного счета без истории операций
    /// </summary>
    [JsonProperty("privilege")]
    public PrivilegeDto Privilege { get; set; }
}