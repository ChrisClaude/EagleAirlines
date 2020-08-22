using System;
using System.Threading.Tasks;
using BookingApi.Models;

namespace BookingApi.Data.Repository.CustomerRepo
{
    public interface ICustomerRepo : IDataRepository<Customer>
    {
    }
}