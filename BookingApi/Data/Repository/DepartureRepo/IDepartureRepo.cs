using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingApi.Models;

namespace BookingApi.Data.Repository.DepartureRepo
{
    interface IDepartureRepo
    {
        IEnumerable<Departure> GetDepartures();
        Departure GetDeparture(int id);
        void CreateDeparture(Departure departure);
        void UpdateDeparture(Departure departure);
        void DeleteDeparture(Departure departure);
        bool SavaeChanges();
    }
}
