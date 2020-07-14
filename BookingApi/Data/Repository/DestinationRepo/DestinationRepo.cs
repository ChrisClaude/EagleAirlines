using BookingApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingApi.Data.Util;

namespace BookingApi.Data.Repository.DestinationRepo
{
    public class DestinationRepo : IDestinationRepo
    {
        public DestinationRepo(BookingContext context)
        {
            _context = context;
        }

        private BookingContext _context { get; }

        public async Task<IEnumerable<Destination>> GetAllAsync(QueryStringParameters parameters)
        {
            IQueryable<Destination> departuresIq;

            // search
            if (!string.IsNullOrEmpty(parameters.SearchString))
            {
                var searchDate = DateTime.Parse(parameters.SearchString);
                departuresIq = _context.Destinations.Where(d => d.Date.Equals(searchDate));
            }
            else
            {
                departuresIq = from d in _context.Destinations select d;
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
            IEnumerable<Destination> departures =
                await PagedList<Destination>.CreateAsync(departuresIq, parameters.PageNumber, parameters.PageSize);

            return departures;
        }

        public async Task<Destination> GetByIdAsync(int id)
        {
            return await _context.Destinations.FindAsync(id);
        }

        public async Task CreateAsync(Destination departure)
        {
            if (departure == null)
            {
                throw new ArgumentNullException(nameof(departure));
            }

            await _context.Destinations.AddAsync(departure);
        }

        public void Update(Destination departure)
        {
            _context.Entry(departure).State = EntityState.Modified;
        }

        public void Delete(Destination departure)
        {
            if (departure == null)
            {
                throw new ArgumentNullException(nameof(departure));
            }

            _context.Destinations.Remove(departure);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}