using System;
using System.Threading.Tasks;
using BookingApi.Models;

namespace BookingApi.Data.Repository.BookingRepo
{
    public interface IBookingRepo : IDataRepository<Booking>
    {
        // Task<Booking> GetByIdAsync(Guid id);
    }
}