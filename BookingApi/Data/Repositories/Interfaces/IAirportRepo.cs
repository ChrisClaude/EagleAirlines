using BookingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApi.Data.Repositories
{
    public interface IAirportRepo
    {
        bool SaveChanges();
        IEnumerable<Airport> GetAllAirports();
        Airport GetAirportById(int id);
        void CreateAirport(Airport airport);
        void UpdateAirport(Airport airport);
        void DeleteAirport(Airport airport);
    }
}
