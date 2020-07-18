using System;
using System.ComponentModel.DataAnnotations;
using BookingApi.Dtos.AirportDto;

namespace BookingApi.Dtos.DepartureDto
{
    public class DepartureReadDto
    {
        public int ID { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        public int FlightID { get; set; }
        public AirportReadDto Airport { get; set; }
    }
}