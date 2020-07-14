using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using BookingApi.Models;
using AutoMapper;
using BookingApi.Dtos;
using BookingApi.Data.Repository.AirportRepo;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using BookingApi.Data.Util;
using BookingApi.Dtos.AirportDto;
using Newtonsoft.Json;

namespace BookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportsController : ControllerBase
    {
        private readonly IAirportRepo _repository;
        private readonly IMapper _mapper;

        public AirportsController(IAirportRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Airports?search=France&sort=name?pageIndex=3&pageSize=35
        /// <summary>
        /// Get all airports from the database. 
        /// </summary>
        /// <param name="search">search by airport name, country or city. e.g: search=Chicago</param>
        /// <param name="sort">sort the returned data by "name", "name_desc", "country", "country_desc", "city", "city_desc". 
        ///     e.g: sort=country would ascending-ly sort the returned data by country name.</param>
        /// <param name="pageIndex">this is the page number of the returned data</param>
        /// <param name="pageSize">this is the number of returned items in the response</param>
        /// <returns>An array of airport objects</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Airport>>> GetAllAirports(string search, string sort, int pageIndex = 1, int pageSize = 25)
        {            
            QueryStringParameters parameters = new AirportParameters();
            parameters.SearchString = search;
            parameters.SortString = sort;
            parameters.PageNumber = pageIndex;
            parameters.PageSize = pageSize;
            
            var airports = await _repository.GetAllAsync(parameters);
                        
            var metadata = new 
            {
                ((PagedList<Airport>) airports).ItemCount,
                parameters.PageSize,
                ((PagedList<Airport>) airports).PageIndex,
                ((PagedList<Airport>) airports).TotalPages,
                ((PagedList<Airport>) airports).HasNextPage,
                ((PagedList<Airport>) airports).HasPreviousPage
            };


            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(_mapper.Map<IEnumerable<AirportReadDto>>(airports));
        }

        // GET: api/Airports/5
        /// <summary>
        /// Get an airport by its id
        /// </summary>
        /// <param name="id">the id of the airport requested</param>
        /// <returns>An airport object</returns>
        [HttpGet("{id:int}", Name = "GetAirport")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Airport>> GetAirportAsync(int id)
        {
            var airport = await _repository.GetByIdAsync(id);

            if (airport == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AirportReadDto>(airport));
        }

        // PUT: api/Airports/5
        /// <summary>
        /// Updates an airport object
        /// </summary>
        /// <param name="id">the id of the airport to update</param>
        /// <param name="airportUpdateDto">the updated object</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateAirportAsync(int id, AirportUpdateDto airportUpdateDto)
        {
            if (id != airportUpdateDto.ID)
            {
                return BadRequest();
            }

            var airportFromRepo = await _repository.GetByIdAsync(id);

            if (airportFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(airportUpdateDto, airportFromRepo);
            //_repository.UpdateAirport(airportFromRepo.Value); // this is achieved by the previous code

            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if ((await _repository.GetByIdAsync(id)) == null)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        // PATCH api/commands/{id}
        /// <summary>
        /// partially updates an airport
        /// </summary>
        /// <param name="id">the id of the airport to update</param>
        /// <param name="patchDoc">the json object with the specific attribute to be updated</param>
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PartialAirportUpdateAsync(int id, JsonPatchDocument<AirportUpdateDto> patchDoc)
        {
            var airportModelFromRepo = await _repository.GetByIdAsync(id);
            if (airportModelFromRepo == null)
            {
                return NotFound();
            }

            var airportToPatch = _mapper.Map<AirportUpdateDto>(airportModelFromRepo);

            patchDoc.ApplyTo(airportToPatch, ModelState);

            if (!TryValidateModel(airportToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(airportToPatch, airportModelFromRepo);
            _repository.Update(airportModelFromRepo);

            await _repository.SaveChangesAsync();

            return NoContent();
        }
        
        // POST: api/Airports
        /// <summary>
        /// Creates an airport 
        /// </summary>
        /// <param name="airportCreateDto">The airport object to create.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Airport>> CreateAirportAsync(AirportCreateDto airportCreateDto)
        {
            var airportModel = _mapper.Map<Airport>(airportCreateDto);
            await _repository.CreateAsync(airportModel);
            await _repository.SaveChangesAsync();

            var airportReadDto = _mapper.Map<AirportReadDto>(airportModel);

            return CreatedAtRoute(nameof(GetAirportAsync), new { Id = airportReadDto.ID }, airportReadDto);
        }
        
        // DELETE: api/Airports/5
        /// <summary>
        /// Deletes an airport
        /// </summary>
        /// <param name="id">id of the object to be deleted</param>
        /// <returns>the deleted object</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Airport>> DeleteAirportAsync(int id)
        {
            var airport = await _repository.GetByIdAsync(id);
            if (airport == null)
            {
                return NotFound();
            }

            _repository.Delete(airport);

            await _repository.SaveChangesAsync();

            return Ok(_mapper.Map<AirportReadDto>(airport));
        }

    }
}
