using BookingApi.Dtos.FlightDto;
using BookingApi.Models;
using System.ComponentModel.DataAnnotations;

namespace BookingApi.Dtos.SeatDto
{
    public class SeatReadDto
    {
        public int Id { get; set; }
        public string SeatNum { get; set; }
        public Cabin Cabin { get; set; }
        public int FlightId { get; set; }

        //public FlightReadDto Flight { get; set; }
    }
}