using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApi.Models
{
    public class Seat
    {
        public int ID { get; set; }
        public string SeatNum { get; set; }
        public Cabin Cabin { get; set; }
        public int FlightID { get; set; }

        public Flight Flight { get; set; }

    }
}
