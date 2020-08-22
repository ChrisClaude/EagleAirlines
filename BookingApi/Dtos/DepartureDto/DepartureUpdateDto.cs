using System;
using System.ComponentModel.DataAnnotations;

namespace BookingApi.Dtos.DepartureDto
{
    public class DepartureUpdateDto
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        public int FlightId { get; set; }
        public int AirportId { get; set; }
    }
}