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

        public async Task<IEnumerable<Flight>> GetAllAsync(QueryStringParameters parameters)
        {
            IQueryable<Flight> flightsIq;

            // search
            if (!string.IsNullOrEmpty(parameters.SearchString))
            {
                var search = parameters.SearchString;
                flightsIq = _context.Flights.Where(f => f.Name.ToUpper().Contains(search.ToUpper())
                                                        || f.Description.ToUpper().Contains(search.ToUpper()));
            }
            else
            {
                flightsIq = from f in _context.Flights select f;
            }


            // sort
            // TODO: this sort is inefficient because is adds unto a big query which is inefficient
            if (!string.IsNullOrEmpty(parameters.SortString))
            {
                var sort = parameters.SortString;

                flightsIq = sort switch
                {
                    "name_desc" => flightsIq.OrderByDescending(f => f.Name),
                    _ => flightsIq.OrderBy(f => f.Name)
                };
            }

            // page
            IEnumerable<Flight> flights =
                await PagedList<Flight>.CreateAsync(flightsIq, parameters.PageNumber, parameters.PageSize);

            return flights;
        }

        public async Task<Flight> GetByIdAsync(int id)
        {
            return await _context.Flights
                .Include(f => f.Departure)
                .ThenInclude(departure => departure.Airport)
                .Include((f => f.Destination))
                .ThenInclude(departure => departure.Airport)
                .AsNoTracking()
                .SingleAsync(f => f.ID == id);
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