using AutoMapper;
using BookingApi.Dtos.PassengerDto;
using BookingApi.Models;

namespace BookingApi.Profiles
{
    public class PassengersProfile : Profile
    {
        public PassengersProfile()
        {
            CreateMap<Passenger, CustomerBookingPassengerReadDto>();
            
            CreateMap<Passenger, PassengerReadDto>();
            CreateMap<PassengerCreateDto, Passenger>();
            CreateMap<PassengerUpdateDto, Passenger>();
            CreateMap<Passenger, PassengerUpdateDto>();   
        }
    }
}