using BookingApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApi.Data.Repository.DestinationRepo
{
    public class DestinationRepo : IDestinationRepo
    {

        public DestinationRepo(BookingContext context)
        {
            _context = context;
        }

        public BookingContext _context { get; }

        public void CreateDeparture(Models.Destination destination)
        {
            if (destination == null) 
            {
                throw new ArgumentNullException(nameof(destination));
            }

            _context.Destinations.Add(destination);
        }

        public void DeleteDeparture(Models.Destination destination)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            _context.Destinations.Remove(destination);
        }

        public Models.Destination GetDeparture(int id)
        {
            return _context.Destinations.Find(id);
        }

        public IEnumerable<Models.Destination> GetDepartures()
        {
            return _context.Destinations.ToList();
        }

        public bool SavaeChanges()
        {
            return _context.SaveChanges() > 0;
        }

        public void UpdateDeparture(Models.Destination destination)
        {
            _context.Entry(destination).State = EntityState.Modified;
        }
    }
}
