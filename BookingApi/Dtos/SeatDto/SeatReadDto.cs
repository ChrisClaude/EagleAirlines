using System;

namespace BookingApi.Dtos.SeatDto
{
    public class SeatReadDto
    {
        public int Id { get; set; }
        public string SeatNum { get; set; }
        public Cabin Cabin { get; set; }
        public int FlightId { get; set; }
    }
}