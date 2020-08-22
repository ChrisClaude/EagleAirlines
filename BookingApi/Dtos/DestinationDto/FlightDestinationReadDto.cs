using System;
using System.ComponentModel.DataAnnotations;
using BookingApi.Dtos.AirportDto;

namespace BookingApi.Dtos.DestinationDto
{
    public class FlightDestinationReadDto
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        public AirportReadDto Airport { get; set; }
    }
}