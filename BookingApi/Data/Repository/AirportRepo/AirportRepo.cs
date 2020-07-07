using BookingApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApi.Data.Repository.AirportRepo
{
    public class AirportRepo : IAirportRepo
    {
        public AirportRepo(BookingContext context)
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
            return _context.Airports.Find(id);
        }

        public void UpdateAirport(Airport airport)
        {
            _context.Entry(airport).State = EntityState.Modified;
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
