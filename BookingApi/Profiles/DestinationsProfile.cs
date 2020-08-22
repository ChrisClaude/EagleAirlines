using AutoMapper;
using BookingApi.Dtos.AirportDto;
using BookingApi.Dtos.DepartureDto;
using BookingApi.Dtos.DestinationDto;
using BookingApi.Models;

namespace BookingApi.Profiles
{
    public class DestinationProfile : Profile
    {
        public DestinationProfile()
        {
            CreateMap<Destination, FlightDestinationReadDto>();
            
            CreateMap<Destination, DestinationReadDto>();
            CreateMap<DestinationCreateDto, Destination>();
            CreateMap<DestinationUpdateDto, Destination>();
            CreateMap<Destination, DestinationUpdateDto>();
        }
    }
}