using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApi.Models
{
    public class Departure
    {
        public int ID { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        public int FlightID { get; set; }
        public int AirportID { get; set; }

        public Flight Flight { get; set; }
        public Airport Airport { get; set; }
    }
}
