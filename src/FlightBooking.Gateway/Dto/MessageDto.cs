using Newtonsoft.Json;

namespace FlightBooking.Gateway.Dto;

public class MessageDto
{
    public MessageDto(string message)
    {
        Message = message;
    }

    [JsonProperty("message")]
    public string Message { get; }
}