using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingApi.Data.Util;
using BookingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Data.Repository.FlightRepo
{
    public class FlightRepo : IFlightRepo
    {
        public FlightRepo(BookingContext context)
        {
            _context = context;
        }

        private BookingContext _context { get; }

        public async Task<IEnumerable<Flight>> GetAllAsync(QueryStringParameter parameter)
        {
            IQueryable<Flight> flightsIq;

            // search
            if (!string.IsNullOrEmpty(parameter.SearchString))
            {
                var search = parameter.SearchString;
                flightsIq = _context.Flights.Where(f => f.Name.ToUpper().Contains(search.ToUpper())
                                                        || f.Description.ToUpper().Contains(search.ToUpper()));
            }
            else
            {
                flightsIq = from f in _context.Flights select f;
            }


            // sort
            if (!string.IsNullOrEmpty(parameter.SortString))
            {
                var sort = parameter.SortString;

                flightsIq = sort switch
                {
                    "name_desc" => flightsIq.OrderByDescending(f => f.Name),
                    _ => flightsIq.OrderBy(f => f.Name)
                };
            }

            // page
            IEnumerable<Flight> flights =
                await PagedList<Flight>.CreateAsync(flightsIq, parameter.PageNumber, parameter.PageSize);

            return flights;
        }

        public async Task<Flight> GetByIdAsync(int id)
        {
            return await _context.Flights.FindAsync(id);
        }

        public async Task CreateAsync(Flight flight)
        {
            if (flight == null)
            {
                throw new ArgumentNullException(nameof(flight));
            }
            
            await _context.Flights.AddAsync(flight);
        }

        public void Update(Flight flight)
        {
            _context.Entry(flight).State = EntityState.Modified;
        }

        public void Delete(Flight flight)
        {
            if (flight == null)
            {
                throw new ArgumentNullException(nameof(flight));
            }

            _context.Flights.Remove(flight);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}