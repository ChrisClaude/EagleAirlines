using BookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApi.Data.Repository.AirportRepo
{
    public interface IAirportRepo
    {
        IEnumerable<Airport> GetAllAirports();
        IEnumerable<Airport> SearchAirports(string searchString);
        Airport GetAirportById(int id);
        void CreateAirport(Airport airport);
        void UpdateAirport(Airport airport);
        void DeleteAirport(Airport airport);
        bool SaveChanges();
    }
}
