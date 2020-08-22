using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BookingApi.Dtos.AirportDto;
using BookingApi.Dtos.BookingDto;
using BookingApi.Models;

namespace BookingApi.Dtos.CustomerDto
{
    public class CustomerReadDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public string Address { get; set; }

        public IEnumerable<CustomerBookingReadDto> Bookings { get; set; }

    }
}