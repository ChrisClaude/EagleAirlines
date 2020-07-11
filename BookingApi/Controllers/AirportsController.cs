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


        // GET: api/Airports?sort="coutry_desc"
        /// <summary>
        /// Get all airports from the database
        /// </summary>
        /// <param name="search">search by airport name, country or city. e.g: search=Chicago</param> 
        /// <param name="sort">sort the returned data by "name", "name_desc", "country", "country_desc", "city", "city_desc". 
        /// e.g: sort=country would ascendingly sort the returned data by country name.</param>
        /// <param name="pageIndex">this is the page number of the returned data</param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Airport>>> GetAllAirports(string search, string sort, int pageIndex = 1)
        {
            IEnumerable<Airport> airports = null;


            if (search != null)
            {
                _logger.LogInformation(search);
                // this search is overloaded to paginate data
                airports = await _repository.Search(search, pageIndex);
            } 
            else
            {
                // no search we use our paginate GetAll()
                _logger.LogInformation(pageIndex.ToString());
                //TODO test paginator, and search how to include page info in the returned json
                airports = airports == null ? await _repository.GetAllAsync(pageIndex)
                    : await _repository.GetAllAsync(pageIndex);
            }


            if (sort != null)
            {
                switch (sort)
                {
                    case "name_desc":
                        _logger.LogInformation(sort);
                        airports = airports == null ? (await _repository.GetAllAsync()).OrderByDescending(a => a.Name)
                            : airports.OrderByDescending(a => a.Name);
                        break;
                    case "country":
                        _logger.LogInformation(sort);
                        airports = airports == null ? (await _repository.GetAllAsync()).OrderBy(a => a.Country)
                            : airports.OrderBy(a => a.Country);
                        break;
                    case "country_desc":
                        _logger.LogInformation(sort);
                        airports = airports == null ? (await _repository.GetAllAsync()).OrderByDescending(a => a.Country)
                            : airports.OrderByDescending(a => a.Country);
                        break;
                    case "city":
                        _logger.LogInformation(sort);
                        airports = airports == null ? (await _repository.GetAllAsync()).OrderBy(a => a.City)
                            : airports.OrderBy(a => a.City);
                        break;
                    case "city_desc":
                        _logger.LogInformation(sort);
                        airports = airports == null ? (await _repository.GetAllAsync()).OrderByDescending(a => a.City)
                            : airports.OrderByDescending(a => a.City);
                        break;
                    default:
                        // this is the fall through case - specifically for name_asc
                        _logger.LogInformation("Fall through - " + sort);
                        airports = airports == null ? (await _repository.GetAllAsync()).OrderBy(a => a.Name)
                            : airports.OrderBy(a => a.Name);
                        break;
                }
            }

            if (airports == null)
            {
                airports = await _repository.GetAllAsync();
            }

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
