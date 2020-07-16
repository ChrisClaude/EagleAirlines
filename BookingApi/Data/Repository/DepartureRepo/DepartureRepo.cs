using BookingApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingApi.Data.Util;

namespace BookingApi.Data.Repository.DepartureRepo
{
    public class DepartureRepo : IDepartureRepo
    {
        public DepartureRepo(BookingContext context)
        {
            _context = context;
        }

        private BookingContext _context { get; }

        public async Task<IEnumerable<Departure>> GetAllAsync(QueryStringParameters parameters)
        {
            IQueryable<Departure> departuresIq;

            // search
            if (!string.IsNullOrEmpty(parameters.SearchString))
            {
                var searchDate = DateTime.Parse(parameters.SearchString);
                departuresIq = _context.Departures.Where(d => d.Date.Equals(searchDate));
            }
            else
            {
                departuresIq = from d in _context.Departures select d;
            }

            // page
            IEnumerable<Departure> departures =
                await PaginatedList<Departure>.CreateAsync(departuresIq, parameters.PageNumber, parameters.PageSize);

            // sort - not set
            if (string.IsNullOrEmpty(parameters.SortString)) return departures;
            
            // sort 
            var sort = parameters.SortString;
            
            var count = ((PaginatedList<Departure>) departures).ItemCount;
            var index = ((PaginatedList<Departure>) departures).PageIndex;
            var size = ((PaginatedList<Departure>) departures).PageSize;

            departures = sort switch
            {
                "date_desc" => departures.OrderByDescending(d => d.Date),
                "flight" => departures.OrderBy(d => d.FlightID),
                "flight_desc" => departures.OrderBy(d => d.FlightID),
                "airport" => departures.OrderByDescending(d => d.AirportID),
                "airport_desc" => departures.OrderByDescending(d => d.AirportID),
                _ => departures.OrderBy(d => d.Date)
            };
            
            return PaginatedList<Departure>.ParsePaginatedList(departures, count, index, size);
        }

        public async Task<Departure> GetByIdAsync(int id)
        {
            return await _context.Departures.FindAsync(id);
        }

        public async Task CreateAsync(Departure departure)
        {
            if (departure == null)
            {
                throw new ArgumentNullException(nameof(departure));
            }
            
            await _context.Departures.AddAsync(departure);
        }

        public void Update(Departure departure)
        {
            _context.Entry(departure).State = EntityState.Modified;   
        }

        public void Delete(Departure departure)
        {
            
            if (departure == null)
            {
                throw new ArgumentNullException(nameof(departure));
            }

            _context.Departures.Remove(departure);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
