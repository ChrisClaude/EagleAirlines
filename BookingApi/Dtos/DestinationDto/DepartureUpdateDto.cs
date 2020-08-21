using System;
using System.ComponentModel.DataAnnotations;

namespace BookingApi.Dtos.DestinationDto
{
    public class DestinationUpdateDto
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        public int FlightId { get; set; }
        public int AirportId { get; set; }
    }
}