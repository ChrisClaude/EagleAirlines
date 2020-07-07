using BookingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApi.Data.Repository.DestinationRepo
{
    interface IDestinationRepo
    {
        IEnumerable<Models.Destination> GetDepartures();
        Models.Destination GetDeparture(int id);
        void CreateDeparture(Models.Destination destination);
        void UpdateDeparture(Models.Destination destination);
        void DeleteDeparture(Models.Destination destination);
        bool SavaeChanges();
    }
}
