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


            // sort
            if (!string.IsNullOrEmpty(parameters.SortString))
            {
                var sort = parameters.SortString;

                departuresIq = sort switch
                {
                    "date_desc" => departuresIq.OrderByDescending(d => d.Date),
                    "flight" => departuresIq.OrderBy(d => d.FlightID),
                    "flight_desc" => departuresIq.OrderBy(d => d.FlightID),
                    "airport" => departuresIq.OrderByDescending(d => d.AirportID),
                    "airport_desc" => departuresIq.OrderByDescending(d => d.AirportID),
                    _ => departuresIq.OrderBy(d => d.Date)
                };
            }

            // page
            IEnumerable<Departure> departures =
                await PaginatedList<Departure>.CreateAsync(departuresIq, parameters.PageNumber, parameters.PageSize);

            return departures;
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
