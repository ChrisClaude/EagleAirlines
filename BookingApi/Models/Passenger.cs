﻿using System.ComponentModel.DataAnnotations;

namespace BookingApi.Models
{
    public class Passenger
    {
        public int Id { get; set; }
        [DataType(DataType.EmailAddress)]

        [Required]
        [StringLength(70, MinimumLength = 4)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(70, MinimumLength = 4)]
        public string Surname { get; set; }
        public int Age { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 4)]
        public string PassportNumber { get; set; }
        
        [Required]
        [StringLength(70, MinimumLength = 4)]
        public string Citizenship { get; set; }

        public int BookingId { get; set; }
        public int SeatId { get; set; }
        
        public Booking Booking { get; set; }
        public Seat Seat { get; set; }
    }
}
