using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace BookingApi.Models
{
    public class Seat
    {
        public int Id { get; set; }
        [Required]
        public string SeatNum { get; set; }
        public string Cabin { get; set; }
        public int FlightId { get; set; }
        
        [JsonIgnore]
        public Flight Flight { get; set; }

    }
}
