using System.Runtime.Serialization;

namespace FlightBooking.BonusService.Extensions;

public static class EnumExtensions
{
    public static string GetValue<T>(this T type) where T: Enum
    {
        var enumType = typeof(T);
        var name = Enum.GetName(enumType, type);
        var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
        return enumMemberAttribute.Value;        
    }
}