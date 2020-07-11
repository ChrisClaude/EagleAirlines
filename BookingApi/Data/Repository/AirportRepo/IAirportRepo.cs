using BookingApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingApi.Data.Repository.AirportRepo
{
    public interface IAirportRepo : IDataRepository<Airport>
    {
        Task<IEnumerable<Airport>> GetAllAsync(int pageIndex);
        Task<IEnumerable<Airport>> Search(string search, int pageIndex);
    }
}
