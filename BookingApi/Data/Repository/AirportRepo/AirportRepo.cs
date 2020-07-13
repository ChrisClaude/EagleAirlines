using BookingApi.Data.Util;
using BookingApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApi.Data.Repository.AirportRepo
{
    public class AirportRepo : IAirportRepo
    {
        public AirportRepo(BookingContext context)
        {
            _context = context;
        }

        private BookingContext _context { get; }

        public async Task CreateAsync(Airport airport)
        {
            if (airport == null)
            {
                throw new ArgumentNullException(nameof(airport));
            }

            await _context.Airports.AddAsync(airport);
        }


        public async Task<IEnumerable<Airport>> GetAllAsync(QueryStringParameter parameter)
        {
            IQueryable<Airport> airportsIq;

            // search
            if (!string.IsNullOrEmpty(parameter.SearchString))
            {
                var searchString = parameter.SearchString;
                airportsIq = _context.Airports.Where(a => a.Name.ToUpper().Contains(searchString.ToUpper())
                                                          || a.Country.ToUpper().Contains(searchString.ToUpper())
                                                          || a.City.ToUpper().Contains(searchString.ToUpper()));
            }
            else
            {
                airportsIq = from a in _context.Airports select a;
            }


            // sort
            if (!string.IsNullOrEmpty(parameter.SortString))
            {
                var sort = parameter.SortString;

                airportsIq = sort switch
                {
                    "name_desc" => airportsIq.OrderByDescending(a => a.Name),
                    "country" => airportsIq.OrderBy(a => a.Country),
                    "country_desc" => airportsIq.OrderByDescending(a => a.Country),
                    "city" => airportsIq.OrderBy(a => a.City),
                    "city_desc" => airportsIq.OrderByDescending(a => a.City),
                    _ => airportsIq.OrderBy(a => a.Name)
                };
            }

            // page
            IEnumerable<Airport> airports =
                await PagedList<Airport>.CreateAsync(airportsIq, parameter.PageNumber, parameter.PageSize);

            return airports;
        }

        public async Task<Airport> GetByIdAsync(int id)
        {
            return await _context.Airports.FindAsync(id);
        }

        public void UpdateAirport(Airport airport)
        {
            _context.Entry(airport).State = EntityState.Modified;
        }

        public void DeleteAirport(Airport airport)
        {
            if (airport == null)
            {
                throw new ArgumentNullException(nameof(airport));
            }

            _context.Airports.Remove(airport);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        //public async Task<IEnumerable<Airport>> SearchAsync(string searchString, int pageIndex)
        //{
        //    var airportsIQ = _context.Airports.Where(a => a.Name.ToUpper().Contains(searchString.ToUpper())
        //                                        || a.Country.ToUpper().Contains(searchString.ToUpper())
        //                                        || a.City.ToUpper().Contains(searchString.ToUpper()));


        //    int pageSize = 50;
        //    var airports = await PagedList<Airport>.CreateAsync(airportsIQ, pageIndex, pageSize); ;
        //    return airports;
        //}

        //public async Task<IEnumerable<Airport>> SearchAsync(string searchString)
        //{
        //    return await _context.Airports.Where(a => a.Name.ToUpper().Contains(searchString.ToUpper())
        //                                        || a.Country.ToUpper().Contains(searchString.ToUpper())
        //                                        || a.City.ToUpper().Contains(searchString.ToUpper())).ToListAsync();
        //}
    }
}