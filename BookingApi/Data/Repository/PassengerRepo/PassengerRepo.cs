using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingApi.Data.Util;
using BookingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Data.Repository.PassengerRepo
{
    public class PassengerRepo : IPassengerRepo
    {
        public PassengerRepo(BookingContext context)
        {
            _context = context;
        }

        private BookingContext _context { get; }

        public async Task<IEnumerable<Passenger>> GetAllAsync(QueryStringParameters queryStringParameters)
        {
            IQueryable<Passenger> passengersIq;

            // search
            if (!string.IsNullOrEmpty(queryStringParameters.SearchString))
            {
                var search = queryStringParameters.SearchString;
                passengersIq = _context.Passengers.Where(p => p.PassportNumber == search
                                                              || p.Citizenship == search
                                                              || p.Surname == search
                                                              || p.Name == search)
                    .Include(passenger => passenger.Booking)
                    .ThenInclude(booking => booking.Customer);
            }
            else
            {
                passengersIq = from b in _context.Passengers.AsNoTracking() select b;
            }

            // page
            IEnumerable<Passenger> passengers =
                await PaginatedList<Passenger>.CreateAsync(passengersIq, queryStringParameters.PageNumber,
                    queryStringParameters.PageSize);


            // sort string not set
            if (string.IsNullOrEmpty(queryStringParameters.SortString)) return passengers;

            // sort
            var sort = queryStringParameters.SortString;

            var count = ((PaginatedList<Passenger>) passengers).ItemCount;
            var index = ((PaginatedList<Passenger>) passengers).PageIndex;
            var size = ((PaginatedList<Passenger>) passengers).PageSize;

            passengers = sort switch
            {
                "name_desc" => passengers.OrderByDescending(p => p.Name),
                "surname" => passengers.OrderBy(p => p.Surname),
                "surname_desc" => passengers.OrderByDescending(p => p.Surname),
                "passport" => passengers.OrderBy(p => p.PassportNumber),
                "passport_desc" => passengers.OrderByDescending(p => p.PassportNumber),
                _ => passengers.OrderBy(p => p.Name)
            };

            return PaginatedList<Passenger>.ParsePaginatedList(passengers, count, index, size);
        }

        public async Task<Passenger> GetByIdAsync(int id)
        {
            return await _context.Passengers.SingleOrDefaultAsync(b => b.Id == id);
        }

        public async Task CreateAsync(Passenger passenger)
        {
            if (passenger == null)
            {
                throw new ArgumentNullException(nameof(passenger));
            }

            await _context.Passengers.AddAsync(passenger);
        }

        public void Update(Passenger passenger)
        {
            _context.Entry(passenger).State = EntityState.Modified;
        }

        public void Delete(Passenger passenger)
        {
            if (passenger == null)
            {
                throw new ArgumentNullException(nameof(passenger));
            }

            _context.Passengers.Remove(passenger);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}