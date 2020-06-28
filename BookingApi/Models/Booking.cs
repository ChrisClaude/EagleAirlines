using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApi.Models
{
    public class Booking
    {
        public int ID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Cost { get; set; }
        public string Status { get; set; }
        public int FlightID { get; set; }
        public int PassengerID { get; set; }

        public Passenger Passenger { get; set; }
        public Flight Flight { get; set; }
    }
}
