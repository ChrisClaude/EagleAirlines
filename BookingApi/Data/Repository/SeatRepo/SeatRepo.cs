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
        
        public async Task<IEnumerable<Seat>> GetAllAsync(QueryStringParameter parameter)
        {
            IQueryable<Seat> seatsIq;

            // search
            if (!string.IsNullOrEmpty(parameter.SearchString))
            {
                var search = parameter.SearchString;
                seatsIq = _context.Seats.Where(s => s.SeatNum.ToUpper().Contains(search.ToUpper()));
            }
            else
            {
                seatsIq = from s in _context.Seats select s;
            }

            // sort
            if (!string.IsNullOrEmpty(parameter.SortString))
            {
                var sort = parameter.SortString;

                seatsIq = sort switch
                {
                    "seat_desc" => seatsIq.OrderByDescending(s => s.SeatNum),
                    "cabin" => seatsIq.OrderBy(s => s.Cabin),
                    "cabin_desc" => seatsIq.OrderByDescending(s => s.Cabin),
                    _ => seatsIq.OrderBy(s => s.SeatNum)
                };
            }

            // page
            IEnumerable<Seat> flights =
                await PagedList<Seat>.CreateAsync(seatsIq, parameter.PageNumber, parameter.PageSize);

            return flights;
        }

        public async Task<Seat> GetByIdAsync(int id)
        {
            return await _context.Seats.FindAsync(id);
        }

        public async Task CreateAsync(Seat seat)
        {
            if (seat == null)
            {
                throw new ArgumentNullException(nameof(seat));
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