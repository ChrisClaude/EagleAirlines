using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApi.Models
{
    public class Booking
    {
        // TODO: this ID will have to be changed to accomodate a booking ID like --> RESERVATION CODE BXJHFT 
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }
        [Required]
        public string Status { get; set; } // TODO: Status constants should be added { Confirmed, Not Confirmed }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public IEnumerable<Passenger> Passengers { get; set; }
    }
}
