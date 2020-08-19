using System.ComponentModel.DataAnnotations;

namespace BookingApi.Dtos.SeatDto
{
    public class SeatUpdateDto
    {
        public int Id { get; set; }
        public string SeatNum { get; set; }
        [Range(0, 1)]
        public Cabin Cabin { get; set; }
        public int FlightId { get; set; }
    }
}