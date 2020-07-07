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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Airport>> GetAllAirports(string search, string sort)
        {
            IEnumerable<Airport> airports = null;


            if (search != null)
            {
                _logger.LogInformation(search);
                airports = _repository.SearchAirports(search);
            }
            
            if (sort != null)
            {
                //TODO Adapt this logic to consider that the preceding parameters are passed
                switch (sort)
                {
                    case "name_desc":
                        _logger.LogInformation(sort);
                        airports = airports == null? _repository.GetAllAirports().OrderByDescending(a => a.Name) 
                            : airports.OrderByDescending(a => a.Name);
                        break;
                    case "country":
                        _logger.LogInformation(sort);
                        airports = airports == null? _repository.GetAllAirports().OrderBy(a => a.Country)
                            : airports.OrderBy(a => a.Country);
                        break;
                    case "country_desc":
                        _logger.LogInformation(sort);
                        airports = airports == null? _repository.GetAllAirports().OrderByDescending(a => a.Country)
                            : airports.OrderByDescending(a => a.Country);
                        break;
                    case "city":
                        _logger.LogInformation(sort);
                        airports = airports == null? _repository.GetAllAirports().OrderBy(a => a.City)
                            : airports.OrderBy(a => a.City);
                        break;
                    case "city_desc":
                        _logger.LogInformation(sort);
                        airports = airports == null? _repository.GetAllAirports().OrderByDescending(a => a.City)
                            : airports.OrderByDescending(a => a.City);
                        break;
                    default:
                        // this is the fall through case - specifically for name_asc
                        _logger.LogInformation("Fall through - " + sort);
                        airports = airports == null? _repository.GetAllAirports().OrderBy(a => a.Name)
                            : airports.OrderBy(a => a.Name);
                        break;
                }
            }

            if (airports == null)
            {
                airports = _repository.GetAllAirports();
            }

            return Ok(_mapper.Map<IEnumerable<AirportReadDto>>(airports));
        }

        // GET: api/Airports/5
        [HttpGet("{id:int}", Name = "GetAirport")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Airport> GetAirport(int id)
        {
            var airport = _repository.GetAirportById(id);

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
        public IActionResult UpdateAirport(int id, AirportUpdateDto airportUpdateDto)
        {
            if (id != airportUpdateDto.ID)
            {
                return BadRequest();
            }

            var airportFromRepo = _repository.GetAirportById(id);

            if (airportFromRepo == null) 
            {
                return NotFound();
            }

            _mapper.Map(airportUpdateDto, airportFromRepo);
            //_repository.UpdateAirport(airportFromRepo.Value); // this is achieved by the previous code

            try
            {
                _repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException) when (_repository.GetAirportById(id) == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // PATCH api/commands/{id}
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult PartialAirportUpdate(int id, JsonPatchDocument<AirportUpdateDto> patchDoc)
        {
            var airportModelFromRepo = _repository.GetAirportById(id);
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

            _repository.SaveChanges();

            return NoContent();
        }


        // POST: api/Airports
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<Airport> CreateAirport(AirportCreateDto airportCreateDto)
        {
            var airportModel = _mapper.Map<Airport>(airportCreateDto);
            _repository.CreateAirport(airportModel);
            _repository.SaveChanges();

            var airportReadDto = _mapper.Map<AirportReadDto>(airportModel);

            return CreatedAtRoute(nameof(GetAirport), new { Id = airportReadDto.ID }, airportReadDto);
        }

        // DELETE: api/Airports/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Airport> DeleteAirport(int id)
        {
            var airport = _repository.GetAirportById(id);
            if (airport == null)
            {
                return NotFound();
            }

            _repository.DeleteAirport(airport);
            
            _repository.SaveChanges();

            return Ok(_mapper.Map<AirportReadDto>(airport));
        }

        //TODO: Add search actions for airport controller
        //TODO: Add Async functionality
    }
}
