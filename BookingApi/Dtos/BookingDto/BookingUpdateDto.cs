using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApi.Dtos.BookingDto
{
    public class BookingUpdateDto
    {
        [DataType(DataType.Date)]
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Cost { get; set; }
        [Required]
        public string Status { get; set; }
        public int FlightId { get; set; }
        public int PassengerId { get; set; }  
    }
}