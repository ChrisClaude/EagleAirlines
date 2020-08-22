using System.Collections.Generic;
using BookingApi.Dtos.BookingDto;

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