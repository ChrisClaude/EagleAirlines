using AutoMapper;
using BookingApi.Dtos;
using BookingApi.Dtos.AirportDto;
using BookingApi.Models;

namespace BookingApi.Profiles
{
    public class AirportProfile : Profile
    {
        public AirportProfile()
        {
            CreateMap<Airport, AirportReadDto>();
            CreateMap<AirportCreateDto, Airport>();
            CreateMap<AirportUpdateDto, Airport>();
            CreateMap<Airport, AirportUpdateDto>();
        }
    }
}
