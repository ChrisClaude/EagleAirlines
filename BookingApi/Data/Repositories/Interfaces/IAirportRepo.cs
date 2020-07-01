using BookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApi.Data.Repositories
{
    public interface IAirportRepo
    {
        bool AirportExists(int id);
        Task<ActionResult<bool>> SaveChanges();
        Task<ActionResult<IEnumerable<Airport>>> GetAllAirports();
        Task<ActionResult<Airport>> GetAirportById(int id);
        void CreateAirport(Airport airport);
        void UpdateAirport(int id, Airport airport);
        void DeleteAirport(Airport airport);
    }
}
