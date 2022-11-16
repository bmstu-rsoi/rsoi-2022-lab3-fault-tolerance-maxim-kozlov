using System.Runtime.Serialization;

namespace FlightBooking.BonusService.Dto;

/// <summary>
/// Типы операций
/// </summary>
public enum OperationTypeDto
{
    [EnumMember(Value = "FILL_IN_BALANCE")]
    FillInBalance = 0,
    
    [EnumMember(Value = "DEBIT_THE_ACCOUNT")]
    DebitAccount = 1
}