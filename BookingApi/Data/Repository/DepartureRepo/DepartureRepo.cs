using BookingApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApi.Data.Repository.DepartureRepo
{
    public class DepartureRepo : IDepartureRepo
    {
        public DepartureRepo(BookingContext context)
        {
            _context = context;
        }

        public BookingContext _context { get; }

        public void CreateDeparture(Departure departure)
        {
            if (departure == null)
            {
                throw new ArgumentNullException(nameof(departure));
            }

            _context.Departures.Add(departure);
        }

        public void DeleteDeparture(Departure departure)
        {
            if (departure == null)
            {
                throw new ArgumentNullException(nameof(departure));
            }

            _context.Departures.Remove(departure);
        }

        public Departure GetDeparture(int id)
        {
            return _context.Departures.Find(id);
        }

        public IEnumerable<Departure> GetDepartures()
        {
            return _context.Departures.ToList();
        }

        public bool SavaeChanges()
        {
            return _context.SaveChanges() > 0;
        }

        public void UpdateDeparture(Departure departure)
        {
            _context.Entry(departure).State = EntityState.Modified;
        }
    }
}
