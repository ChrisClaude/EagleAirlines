using BookingApi.Models;
using Microsoft.AspNetCore.Mvc;
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

        public ActionResult<IEnumerable<Airport>> GetAllAirports()
        {
            return _context.Airports.ToList();
        }

        public ActionResult<Airport> GetAirportById(int id)
        {
            return _context.Airports.Find(id);
        }

        public void UpdateAirport(int id, Airport airport)
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

        public ActionResult<bool> SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public bool AirportExists(int id)
        {
            return _context.Airports.Any(e => e.ID == id);
        }
    }
}
