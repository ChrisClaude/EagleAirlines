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
        // TODO: this ID will have to be changed to accomodate a booking ID like --> RESERVATION CODE BXJHFT 
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Cost { get; set; }
        public string Status { get; set; }
        public int FlightId { get; set; }
        public int PassengerId { get; set; }

        public Passenger Passenger { get; set; }
        public Flight Flight { get; set; }
    }
}
