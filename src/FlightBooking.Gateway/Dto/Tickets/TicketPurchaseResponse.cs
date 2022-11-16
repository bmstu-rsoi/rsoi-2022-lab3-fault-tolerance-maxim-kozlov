using System;
using FlightBooking.BonusService.Dto;
using FlightBooking.TicketService.Dto;
using Newtonsoft.Json;

namespace FlightBooking.Gateway.Dto.Tickets;

public class TicketPurchaseResponse
{
    /// <summary>
    /// UUID билета
    /// </summary>
    [JsonProperty("ticketUid")]
    public Guid TicketId { get; set; }
    
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
    public DateTimeOffset Date { get; set; }
    
    /// <summary>
    /// Статус билета
    /// </summary>
    [JsonProperty("status")]
    public TicketStatusDto Status { get; set; }
    
    /// <summary>
    /// Стоимость
    /// </summary>
    [JsonProperty("price")]
    public int Price { get; set; }
    
    /// <summary>
    /// Сумма оплаченная деньгами
    /// </summary>
    [JsonProperty("paidByMoney")]
    public int paidByMoney { get; set; }
    
    /// <summary>
    /// Сумма оплаченная бонусами
    /// </summary>
    [JsonProperty("paidByBonuses")]
    public int paidByBonuses { get; set; }
    
    /// <summary>
    /// Баланс бонусного счета без истории операций
    /// </summary>
    [JsonProperty("privilege")]
    public PrivilegeDto Privilege { get; set; }
}