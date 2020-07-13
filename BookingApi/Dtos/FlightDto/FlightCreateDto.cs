using System.ComponentModel.DataAnnotations.Schema;
using BookingApi.Models;

namespace BookingApi.Dtos.FlightDto
{
    public class FlightCreateDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}