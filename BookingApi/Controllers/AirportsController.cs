using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using BookingApi.Models;
using AutoMapper;
using BookingApi.Dtos;
using BookingApi.Dtos.Airport;
using BookingApi.Data.Repository.AirportRepo;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using BookingApi.Data.Util;
using Newtonsoft.Json;

namespace BookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportsController : ControllerBase
    {
        private readonly IAirportRepo _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AirportsController(IAirportRepo repository, IMapper mapper, ILogger<AirportsController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        // GET: api/Airports?search=France&sort=name?pageIndex=3&pageSize=35
        /// <summary>
        /// Get all airports from the database
        /// </summary>
        /// <param name="search">search by airport name, country or city. e.g: search=Chicago</param>
        /// <param name="sort">sort the returned data by "name", "name_desc", "country", "country_desc", "city", "city_desc". 
        ///     e.g: sort=country would ascendingly sort the returned data by country name.</param>
        /// <param name="pageIndex">this is the page number of the returned data</param>
        /// <param name="pageSize">this is the number of returned items in the response</param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Airport>>> GetAllAirports(string search, string sort, int pageIndex = 1, int pageSize = 25)
        {            
            QueryStringParameter parameter = new AirportParameter();
            parameter.SearchString = search;
            parameter.SortString = sort;
            parameter.PageNumber = pageIndex;
            parameter.PageSize = pageSize;
            
            var airports = await _repository.GetAllAsync(parameter);
                        
            var metadata = new 
            {
                ((PagedList<Airport>) airports).ItemCount,
                parameter.PageSize,
                ((PagedList<Airport>) airports).PageIndex,
                ((PagedList<Airport>) airports).TotalPages,
                ((PagedList<Airport>) airports).HasNextPage,
                ((PagedList<Airport>) airports).HasPreviousPage
            };


            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(_mapper.Map<IEnumerable<AirportReadDto>>(airports));
        }

        // GET: api/Airports/5
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
            _repository.UpdateAirport(airportModelFromRepo);

            await _repository.SaveChangesAsync();

            return NoContent();
        }


        // POST: api/Airports
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

            _repository.DeleteAirport(airport);

            await _repository.SaveChangesAsync();

            return Ok(_mapper.Map<AirportReadDto>(airport));
        }

    }
}
