using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingApi.Data.Util;

namespace BookingApi.Data.Repository
{
    public interface IDataRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(QueryStringParameters queryStringParameters);
        //Task<IEnumerable<T>> SearchAsync(string searchString);
        Task<T> GetByIdAsync(int id);
        Task CreateAsync(T t);
        void Update(T t);
        void Delete(T t);
        Task<bool> SaveChangesAsync();
    }
}
