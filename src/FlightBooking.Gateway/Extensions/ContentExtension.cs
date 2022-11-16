using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FlightBooking.Gateway.Extensions;

public static class ContentExtension
{
    public static async Task<T?> ReadAsJsonAsync<T>(this HttpContent content, JsonSerializerSettings? settings = null)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));
        var body = await content.ReadAsStringAsync();

        settings ??= new JsonSerializerSettings();
        settings.Converters.Add(new StringEnumConverter());
        return JsonConvert.DeserializeObject<T>(body, settings);
    }
}