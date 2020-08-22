using System;
using System.ComponentModel.DataAnnotations;
using BookingApi.Dtos.AirportDto;
using BookingApi.Models;

namespace BookingApi.Dtos.DestinationDto
{
    public class DestinationReadDto
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        public int FlightId { get; set; }
        public AirportReadDto Airport { get; set; }
    }
}