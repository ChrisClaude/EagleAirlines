using System;
using System.Threading.Tasks;
using BookingApi.Models;

namespace BookingApi.Data.Repository.BookingRepo
{
    public interface IBookingRepo : IDataRepository<Booking>
    {
        /**
         * This is an overloaded method that caters for Guid keys 
         */
        Task<Booking> GetByIdAsync(Guid id);
    }
}