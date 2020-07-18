using System;
using System.ComponentModel.DataAnnotations;
using BookingApi.Dtos.AirportDto;
using BookingApi.Models;

namespace BookingApi.Dtos.DestinationDto
{
    public class DestinationReadDto
    {
        public int ID { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        public int FlightID { get; set; }
        public AirportReadDto Airport { get; set; }
    }
}