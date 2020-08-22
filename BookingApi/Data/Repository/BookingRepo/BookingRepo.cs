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
                bookingsIq = _context.Bookings
                    .Where(b => b.TimeStamp == DateTime.Parse(search))
                    .Include(booking => booking.Passengers)
                    .ThenInclude(passenger => passenger.Seat)
                    .Include(b => b.Customer)
                    .AsNoTracking();
            }
            else
            {
                bookingsIq = from b in _context.Bookings
                    .Include(booking => booking.Passengers)
                    .ThenInclude(passenger => passenger.Seat)
                    .Include(b => b.Customer)
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

        
        public async Task<Booking> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(booking => booking.Passengers)
                .ThenInclude(passenger => passenger.Seat)
                .Include(b => b.Customer)
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

        public void Update(Booking booking)
        {
            _context.Entry(booking).State = EntityState.Modified;
        }

        public void Delete(Booking booking)
        {
            if (booking == null)
            {
                throw new ArgumentNullException(nameof(booking));
            }

            _context.Bookings.Remove(booking);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}