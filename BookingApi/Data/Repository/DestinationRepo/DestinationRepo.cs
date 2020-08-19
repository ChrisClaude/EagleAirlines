using BookingApi.Models;
using Microsoft.EntityFrameworkCore;
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
            IQueryable<Destination> destinationsIq;

            // search
            if (!string.IsNullOrEmpty(parameters.SearchString))
            {
                var searchDate = DateTime.Parse(parameters.SearchString);
                destinationsIq = _context.Destinations.Where(d => d.Date.Equals(searchDate));
            }
            else
            {
                destinationsIq = from d in _context.Destinations
                        .Include(des => des.Airport)
                    select d;
            }

            // page
            IEnumerable<Destination> destinations =
                await PaginatedList<Destination>.CreateAsync(destinationsIq, parameters.PageNumber,
                    parameters.PageSize);

            // sort - string not set
            if (string.IsNullOrEmpty(parameters.SortString)) return destinations;

            // sort
            var sort = parameters.SortString;

            var count = ((PaginatedList<Destination>) destinations).ItemCount;
            var index = ((PaginatedList<Destination>) destinations).PageIndex;
            var size = ((PaginatedList<Destination>) destinations).PageSize;

            destinations = sort switch
            {
                "date_desc" => destinations.OrderByDescending(d => d.Date),
                "flight" => destinations.OrderBy(d => d.FlightId),
                "flight_desc" => destinations.OrderBy(d => d.FlightId),
                "airport" => destinations.OrderByDescending(d => d.AirportId),
                "airport_desc" => destinations.OrderByDescending(d => d.AirportId),
                _ => destinations.OrderBy(d => d.Date)
            };

            return PaginatedList<Destination>.ParsePaginatedList(destinations, count, index, size);
        }

        public async Task<Destination> GetByIdAsync(int id)
        {
            return await _context.Destinations
                .Include(des => des.Airport)
                .SingleOrDefaultAsync(des => des.Id == id);
        }

        public async Task CreateAsync(Destination destination)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            await _context.Destinations.AddAsync(destination);
        }

        public void Update(Destination destination)
        {
            _context.Entry(destination).State = EntityState.Modified;
        }

        public void Delete(Destination destination)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            _context.Destinations.Remove(destination);
        }

        public async Task<bool> IsFlightIdUnique(int flightId)
        {
            var destination = await _context.Destinations.Where(d => d.FlightId == flightId).SingleOrDefaultAsync();
            return destination == null;
        }
        
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}