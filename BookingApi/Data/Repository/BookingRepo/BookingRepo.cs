using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingApi.Data.Util;
using BookingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Data.Repository.BookingRepo
{
    public class BookingRepo : IBookingRepo
    {
        public BookingRepo(BookingContext context)
        {
            _context = context;
        }

        private BookingContext _context { get; }

        public async Task<IEnumerable<Booking>> GetAllAsync(QueryStringParameters queryStringParameters)
        {
            IQueryable<Booking> bookingsIq;

            // search
            if (!string.IsNullOrEmpty(queryStringParameters.SearchString))
            {
                var search = queryStringParameters.SearchString;
                // search by date
                bookingsIq = _context.Bookings.Where(b => b.TimeStamp == DateTime.Parse(search));
            }
            else
            {
                bookingsIq = from b in _context.Bookings
                    .Include(booking => booking.Passengers)
                    .Include(booking => booking.Seats)
                    .ThenInclude(seat => seat.Flight)
                    .AsNoTracking()
                    select b;
            }

            // page
            IEnumerable<Booking> bookings =
                await PaginatedList<Booking>.CreateAsync(bookingsIq, queryStringParameters.PageNumber, queryStringParameters.PageSize);


            // sort string not set
            if (string.IsNullOrEmpty(queryStringParameters.SortString)) return bookings;
            
            // sort
            var sort = queryStringParameters.SortString;

            var count = ((PaginatedList<Booking>) bookings).ItemCount;
            var index = ((PaginatedList<Booking>) bookings).PageIndex;
            var size = ((PaginatedList<Booking>) bookings).PageSize;
            
            bookings = sort switch
            {
                "timestamp_desc" => bookings.OrderByDescending(b => b.TimeStamp),
                "cost" => bookings.OrderBy(b => b.Cost),
                "cost_desc" => bookings.OrderByDescending(b => b.Cost),
                _ => bookings.OrderBy(b => b.TimeStamp)
            };
            
            return PaginatedList<Booking>.ParsePaginatedList(bookings, count, index, size);
        }

        public Task<Booking> GetByIdAsync(int id)
        {
            return null;
        }
        
        public async Task<Booking> GetByIdAsync(Guid id)
        {
            return await _context.Bookings
                .Include(booking => booking.Passengers)
                .Include(booking => booking.Seats)
                .ThenInclude(seat => seat.Flight)
                .SingleOrDefaultAsync(b => b.Id == id);
        }

        public async Task CreateAsync(Booking booking)
        {
            if (booking == null)
            {
                throw new ArgumentNullException(nameof(booking));
            }

            await _context.Bookings.AddAsync(booking);
        }

        public void Update(Booking flight)
        {
            _context.Entry(flight).State = EntityState.Modified;
        }

        public void Delete(Booking flight)
        {
            if (flight == null)
            {
                throw new ArgumentNullException(nameof(flight));
            }

            _context.Bookings.Remove(flight);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}