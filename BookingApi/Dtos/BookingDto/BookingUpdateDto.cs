using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApi.Dtos.BookingDto
{
    public class BookingUpdateDto
    {
        public Guid Id { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Cost { get; set; }
        [Required]
        public string Status { get; set; }
    }
}