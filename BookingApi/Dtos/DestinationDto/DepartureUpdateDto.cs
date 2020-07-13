using System;
using System.ComponentModel.DataAnnotations;

namespace BookingApi.Dtos.DepartureDto
{
    public class DepartureUpdateDto
    {
        public int ID { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        public int FlightID { get; set; }
        public int AirportID { get; set; }
    }
}