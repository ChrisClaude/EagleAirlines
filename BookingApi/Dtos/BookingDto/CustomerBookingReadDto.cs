using System;
using System.Collections.Generic;
using BookingApi.Dtos.PassengerDto;

namespace BookingApi.Dtos.BookingDto
{
    public class CustomerBookingReadDto
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public decimal Cost { get; set; }
        public string Status { get; set; }
        
        public IEnumerable<CustomerBookingPassengerReadDto> Passengers { get; set; }
    }
}