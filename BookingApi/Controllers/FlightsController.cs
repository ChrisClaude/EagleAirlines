using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookingApi.Data.Repository.FlightRepo;
using BookingApi.Data.Util;
using BookingApi.Dtos.FlightDto;
using BookingApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightRepo _repository;
        private readonly IMapper _mapper;

        public FlightsController(IFlightRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Flights?search=France&sort=name?pageIndex=3&pageSize=35
        /// <summary>
        /// Get all flights from the database. 
        /// </summary>
        /// <param name="search">search by flight name, country or city. e.g: search=Chicago</param>
        /// <param name="sort">sort the returned data by "name", "name_desc", "country", "country_desc", "city", "city_desc". 
        ///     e.g: sort=country would ascending-ly sort the returned data by country name.</param>
        /// <param name="pageIndex">this is the page number of the returned data</param>
        /// <param name="pageSize">this is the number of returned items in the response</param>
        /// <returns>An array of flight objects</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Flight>>> GetAllFlights(string search, string sort, int pageIndex = 1, int pageSize = 25)
        {            
            QueryStringParameters parameters = new FlightParameters();
            parameters.SearchString = search;
            parameters.SortString = sort;
            parameters.PageNumber = pageIndex;
            parameters.PageSize = pageSize;
            
            var flights = await _repository.GetAllAsync(parameters);
                        
            var metadata = new 
            {
                ((PaginatedList<Flight>) flights).ItemCount,
                parameters.PageSize,
                ((PaginatedList<Flight>) flights).PageIndex,
                ((PaginatedList<Flight>) flights).TotalPages,
                ((PaginatedList<Flight>) flights).HasNextPage,
                ((PaginatedList<Flight>) flights).HasPreviousPage
            };


            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(_mapper.Map<IEnumerable<FlightReadDto>>(flights));
        }

        // GET: api/Flights/5
        /// <summary>
        /// Get an flight by its id
        /// </summary>
        /// <param name="id">the id of the flight requested</param>
        /// <returns>An flight object</returns>
        [HttpGet("{id:int}", Name = "GetFlightAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Flight>> GetFlightAsync(int id)
        {
            var flight = await _repository.GetByIdAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<FlightReadDto>(flight));
        }

        // PUT: api/Flights/5
        /// <summary>
        /// Updates an flight object
        /// </summary>
        /// <param name="id">the id of the flight to update</param>
        /// <param name="flightUpdateDto">the updated object</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateFlightAsync(int id, FlightUpdateDto flightUpdateDto)
        {
            if (id != flightUpdateDto.ID)
            {
                return BadRequest();
            }

            var flightFromRepo = await _repository.GetByIdAsync(id);

            if (flightFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(flightUpdateDto, flightFromRepo);
            //_repository.UpdateFlight(flightFromRepo.Value); // this is achieved by the previous code

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
        /// partially updates an flight
        /// </summary>
        /// <param name="id">the id of the flight to update</param>
        /// <param name="patchDoc">the json object with the specific attribute to be updated</param>
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PartialFlightUpdateAsync(int id, JsonPatchDocument<FlightUpdateDto> patchDoc)
        {
            var flightModelFromRepo = await _repository.GetByIdAsync(id);
            if (flightModelFromRepo == null)
            {
                return NotFound();
            }

            var flightToPatch = _mapper.Map<FlightUpdateDto>(flightModelFromRepo);

            patchDoc.ApplyTo(flightToPatch, ModelState);

            if (!TryValidateModel(flightToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(flightToPatch, flightModelFromRepo);
            _repository.Update(flightModelFromRepo);

            await _repository.SaveChangesAsync();

            return NoContent();
        }
        
        // POST: api/Flights
        /// <summary>
        /// Creates an flight 
        /// </summary>
        /// <param name="flightCreateDto">The flight object to create.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Flight>> CreateFlightAsync(FlightCreateDto flightCreateDto)
        {
            var flightModel = _mapper.Map<Flight>(flightCreateDto);
            await _repository.CreateAsync(flightModel);
            await _repository.SaveChangesAsync();

            var flightReadDto = _mapper.Map<FlightReadDto>(flightModel);

            return CreatedAtRoute(nameof(GetFlightAsync), new { Id = flightReadDto.ID }, flightReadDto);
        }
        
        // DELETE: api/Flights/5
        /// <summary>
        /// Deletes an flight
        /// </summary>
        /// <param name="id">id of the object to be deleted</param>
        /// <returns>the deleted object</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Flight>> DeleteFlightAsync(int id)
        {
            var flight = await _repository.GetByIdAsync(id);
            if (flight == null)
            {
                return NotFound();
            }

            _repository.Delete(flight);

            await _repository.SaveChangesAsync();

            return Ok(_mapper.Map<FlightReadDto>(flight));
        }

    }
}