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
        [Key]
        public Guid Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Cost { get; set; }
        [Required]
        public string Status { get; set; } // TODO: Status constants should be added { Confirmed, Not Confirmed }

        public IEnumerable<Passenger> Passengers { get; set; }
        public IEnumerable<Seat> Seats { get; set; }
    }
}
