using AutoMapper;
using BookingApi.Dtos.AirportDto;
using BookingApi.Dtos.DepartureDto;
using BookingApi.Models;

namespace BookingApi.Profiles
{
    public class DepartureProfile : Profile
    {
        public DepartureProfile()
        {
            CreateMap<Departure, DepartureReadDto>();
            CreateMap<DepartureCreateDto, Departure>();
            CreateMap<DepartureUpdateDto, Departure>();
            CreateMap<Departure, DepartureUpdateDto>();
        }
    }
}