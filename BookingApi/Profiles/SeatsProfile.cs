using AutoMapper;
using BookingApi.Models;
using BookingApi.Dtos.SeatDto;


namespace BookingApi.Profiles
{
    public class SeatsProfile : Profile
    {
        public SeatsProfile()
        {
            CreateMap<Seat, SeatReadDto>();
            CreateMap<SeatCreateDto, Seat>();
            CreateMap<SeatUpdateDto, Seat>();
            CreateMap<Seat, SeatUpdateDto>();
        }
    }
}