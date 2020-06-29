using BookingApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApi.Data.Repositories
{
    public class SqlAirportRepo : IAirportRepo
    {
        public SqlAirportRepo(BookingContext context)
        {
            _context = context;
        }

        public BookingContext _context { get; }

        public void CreateAirport(Airport airport)
        {
            if (airport == null)
            {
                throw new ArgumentNullException(nameof(airport));
            }

            _context.Airports.Add(airport);
        }

        public IEnumerable<Airport> GetAllAirports()
        {
            return _context.Airports.ToList();
        }

        public Airport GetAirportById(int id)
        {
            return _context.Airports.FirstOrDefault(p => p.ID == id);
        }

        public void UpdateAirport(Airport airport)
        {
            // Nothing
        }

        public void DeleteAirport(Airport airport)
        {
            if (airport == null)
            {
                throw new ArgumentNullException(nameof(airport));
            }

            _context.Airports.Remove(airport);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
