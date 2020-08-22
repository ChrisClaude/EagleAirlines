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

        public async Task<IEnumerable<Flight>> GetAllAsync(QueryStringParameters queryStringParameters)
        {
            IQueryable<Flight> flightsIq;

            // search
            if (!string.IsNullOrEmpty(queryStringParameters.SearchString))
            {
                var search = queryStringParameters.SearchString;
                flightsIq = _context.Flights
                    .Where(f => f.Name.ToUpper().Contains(search.ToUpper()) || f.Description.ToUpper().Contains(search.ToUpper()))
                    .Include(f => f.Departure)
                    .ThenInclude(departure => departure.Airport)
                    .Include(f => f.Destination)
                    .ThenInclude(destination => destination.Airport)
                    .AsNoTracking();
            }
            else
            {
                flightsIq = from f in _context.Flights
                    .Include(f => f.Departure)
                    .ThenInclude(departure => departure.Airport)
                    .Include(f => f.Destination)
                    .ThenInclude(destination => destination.Airport)
                    .AsNoTracking()
                    select f;
            }

            // page
            IEnumerable<Flight> flights =
                await PaginatedList<Flight>.CreateAsync(flightsIq, queryStringParameters.PageNumber, queryStringParameters.PageSize);


            // sort string not set
            if (string.IsNullOrEmpty(queryStringParameters.SortString)) return flights;
            
            // sort
            var sort = queryStringParameters.SortString;

            var count = ((PaginatedList<Flight>) flights).ItemCount;
            var index = ((PaginatedList<Flight>) flights).PageIndex;
            var size = ((PaginatedList<Flight>) flights).PageSize;
            
            flights = sort switch
            {
                "name_desc" => flights.OrderByDescending(f => f.Name),
                _ => flights.OrderBy(f => f.Name)
            };
            
            return PaginatedList<Flight>.ParsePaginatedList(flights, count, index, size);
        }

        public async Task<Flight> GetByIdAsync(int id)
        {
            return await _context.Flights
                .Include(f => f.Departure)
                .ThenInclude(departure => departure.Airport)
                .Include((f => f.Destination))
                .ThenInclude(destination => destination.Airport)
                .AsNoTracking()
                .SingleOrDefaultAsync(f => f.Id == id);
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