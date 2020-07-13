using BookingApi.Models;

namespace BookingApi.Dtos.FlightDto
{
    public class FlightUpdateDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}