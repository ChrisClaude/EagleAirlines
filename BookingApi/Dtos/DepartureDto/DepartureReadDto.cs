using System;
using System.ComponentModel.DataAnnotations;
using BookingApi.Dtos.AirportDto;

namespace BookingApi.Dtos.DepartureDto
{
    public class DepartureReadDto
    {
        public int Id { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        public int FlightId { get; set; }
        public AirportReadDto Airport { get; set; }
    }
}