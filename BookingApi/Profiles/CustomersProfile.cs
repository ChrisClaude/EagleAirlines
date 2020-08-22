using AutoMapper;
using BookingApi.Dtos.CustomerDto;
using BookingApi.Models;

namespace BookingApi.Profiles
{
    public class CustomersProfile : Profile
    {
        public CustomersProfile()
        {
            CreateMap<Customer, CustomerReadDto>();
            CreateMap<Customer, BookingCustomerReadDto>();
            CreateMap<CustomerCreateDto, Customer>();
            CreateMap<CustomerUpdateDto, Customer>();
            CreateMap<Customer, CustomerUpdateDto>();   
        }        
    }
}