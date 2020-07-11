using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApi.Data.Repository
{
    public interface IDataRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> Search(string searchString);
        Task<T> GetByIdAsync(int id);
        Task CreateAsync(T t);
        void UpdateAirport(T t);
        void DeleteAirport(T t);
        Task<bool> SaveChangesAsync();
    }
}
