using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookingApi.Models;

namespace BookingApi.Dtos.BookingDto
{
    public class BookingReadDto
    {
        [Key]
        public Guid Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

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