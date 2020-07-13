using AutoMapper;
using BookingApi.Dtos.FlightDto;
using BookingApi.Models;

namespace BookingApi.Profiles
{
    public class FlightsProfile : Profile
    {
        public FlightsProfile()
        {
            CreateMap<Flight, FlightReadDto>();
            CreateMap<FlightCreateDto, Flight>();
            CreateMap<FlightUpdateDto, Flight>();
            CreateMap<Flight, FlightUpdateDto>();   
        }
    }
}