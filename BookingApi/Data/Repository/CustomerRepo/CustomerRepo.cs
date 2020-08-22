using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingApi.Data.Util;
using BookingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Data.Repository.CustomerRepo
{
    public class CustomerRepo : ICustomerRepo
    {
        public CustomerRepo(BookingContext context)
        {
            _context = context;
        }

        private BookingContext _context { get; }

        public async Task<IEnumerable<Customer>> GetAllAsync(QueryStringParameters queryStringParameters)
        {
            IQueryable<Customer> customersIq;

            // search
            if (!string.IsNullOrEmpty(queryStringParameters.SearchString))
            {
                var search = queryStringParameters.SearchString;
                // search by date
                customersIq = _context.Customers
                    .Where(c => c.Name == search || c.Email == search)
                    .Include(customer => customer.Bookings)
                    .ThenInclude(booking => booking.Passengers)
                    .AsNoTracking();
            }
            else
            {
                customersIq = from c in _context.Customers
                        .Include(customer => customer.Bookings)
                        .ThenInclude(booking => booking.Passengers)
                        .AsNoTracking()
                    select c;
            }

            // page
            IEnumerable<Customer> customers =
                await PaginatedList<Customer>.CreateAsync(customersIq, queryStringParameters.PageNumber,
                    queryStringParameters.PageSize);


            // sort string not set
            if (string.IsNullOrEmpty(queryStringParameters.SortString)) return customers;

            // sort
            var sort = queryStringParameters.SortString;

            var count = ((PaginatedList<Customer>) customers).ItemCount;
            var index = ((PaginatedList<Customer>) customers).PageIndex;
            var size = ((PaginatedList<Customer>) customers).PageSize;

            customers = sort switch
            {
                "name_desc" => customers.OrderByDescending(c => c.Name),
                "email" => customers.OrderBy(c => c.Email),
                "email_desc" => customers.OrderByDescending(c => c.Email),
                _ => customers.OrderBy(c => c.Name)
            };

            return PaginatedList<Customer>.ParsePaginatedList(customers, count, index, size);
        }


        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.Customers
                .Include(customer => customer.Bookings)
                .ThenInclude(booking => booking.Passengers)
                .SingleOrDefaultAsync(b => b.Id == id);
        }

        public async Task CreateAsync(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            await _context.Customers.AddAsync(customer);
        }

        public void Update(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
        }

        public void Delete(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            _context.Customers.Remove(customer);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}