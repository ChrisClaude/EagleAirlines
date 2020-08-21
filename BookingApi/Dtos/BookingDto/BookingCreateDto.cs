using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookingApi.Models;

namespace BookingApi.Dtos.BookingDto
{
    public class BookingCreateDto
    {
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Cost { get; set; }
        [Required]
        public string Status { get; set; }
    }
}