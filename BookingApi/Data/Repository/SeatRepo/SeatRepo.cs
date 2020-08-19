using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingApi.Data.Util;
using BookingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Data.Repository.SeatRepo
{
    public class SeatRepo : ISeatRepo
    {
        public SeatRepo(BookingContext context)
        {
            _context = context;
        }

        private BookingContext _context { get; }

        public async Task<IEnumerable<Seat>> GetAllAsync(QueryStringParameters parameters)
        {
            IQueryable<Seat> seatsIq;

            // search
            if (!string.IsNullOrEmpty(parameters.SearchString))
            {
                var search = parameters.SearchString;
                seatsIq = _context.Seats.Where(s => s.SeatNum.ToUpper().Contains(search.ToUpper()));
            }
            else
            {
                seatsIq = from s in _context.Seats select s;
            }

            // page
            IEnumerable<Seat> seats =
                await PaginatedList<Seat>.CreateAsync(seatsIq, parameters.PageNumber, parameters.PageSize);


            // sort - string not set
            if (string.IsNullOrEmpty(parameters.SortString)) return seats;

            // sort
            var sort = parameters.SortString;

            var count = ((PaginatedList<Seat>) seats).ItemCount;
            var index = ((PaginatedList<Seat>) seats).PageIndex;
            var size = ((PaginatedList<Seat>) seats).PageSize;

            seats = sort switch
            {
                "seat_desc" => seats.OrderByDescending(s => s.SeatNum),
                "cabin" => seats.OrderBy(s => s.Cabin),
                "cabin_desc" => seats.OrderByDescending(s => s.Cabin),
                _ => seats.OrderBy(s => s.SeatNum)
            };

            return PaginatedList<Seat>.ParsePaginatedList(seats, count, index, size);
        }

        public async Task<Seat> GetByIdAsync(int id)
        {
            return await _context.Seats
                .Include(s => s.Flight)
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task CreateAsync(Seat seat)
        {
            if (seat == null)
            {
                throw new ArgumentNullException(nameof(seat));
            }

            Seat duplicatedSeat = await _context.Seats.Where(s => s.SeatNum == seat.SeatNum && s.FlightId == seat.FlightId).FirstOrDefaultAsync();

            if (duplicatedSeat != null)
            {
                throw new ArgumentException(nameof(seat));
            }

            await _context.Seats.AddAsync(seat);
        }

        public void Update(Seat seat)
        {
            _context.Entry(seat).State = EntityState.Modified;
        }

        public void Delete(Seat seat)
        {
            if (seat == null)
            {
                throw new ArgumentNullException(nameof(seat));
            }

            _context.Seats.Remove(seat);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}