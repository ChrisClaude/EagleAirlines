using AutoMapper;
using BookingApi.Dtos.BookingDto;
using BookingApi.Models;

namespace BookingApi.Profiles
{
    public class BookingsProfile: Profile
    {
        public BookingsProfile()
        {
            CreateMap<Booking, CustomerBookingReadDto>();
            
            CreateMap<Booking, BookingReadDto>();
            CreateMap<BookingCreateDto, Booking>();
            CreateMap<BookingUpdateDto, Booking>();
            CreateMap<Booking, BookingUpdateDto>();   
        }
    }
}