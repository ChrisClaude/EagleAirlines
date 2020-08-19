using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BookingApi.Dtos.SeatDto
{
 [JsonConverter(typeof(StringEnumConverter))]
    public enum Cabin {
        [EnumMember(Value = "Eco")]
        Eco,
        [EnumMember(Value = "Bus")]
        Bus
    }
}
