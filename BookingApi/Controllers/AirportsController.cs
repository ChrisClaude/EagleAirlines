using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using BookingApi.Data;
using BookingApi.Models;
using BookingApi.Data.Repositories;
using AutoMapper;
using BookingApi.Dtos;
using BookingApi.Dtos.Airport;
using Microsoft.AspNetCore.Authorization;

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

        // GET: api/Airports
        [HttpGet]
        public ActionResult<IEnumerable<Airport>> GetAllAirports()
        {
            var airports = _repository.GetAllAirports();
            return Ok(_mapper.Map<IEnumerable<AirportReadDto>>(airports.Value));
        }

        // GET: api/Airports/5
        [HttpGet("{id}", Name = "GetAirport")]
        public ActionResult<Airport> GetAirport(int id)
        {
            var airport = _repository.GetAirportById(id);

            if (airport == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AirportReadDto>(airport.Value));
        }

        // PUT: api/Airports/5
        [HttpPut("{id}")]
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

            _mapper.Map(airportUpdateDto, airportFromRepo.Value);
            //_repository.UpdateAirport(id, airportFromRepo.Value); // this is achieved by the previous code

            try
            {
                _repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException) when (!_repository.AirportExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // PATCH api/commands/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialAirportUpdate(int id, JsonPatchDocument<AirportUpdateDto> patchDoc)
        {
            var airportModelFromRepo = _repository.GetAirportById(id);
            if (airportModelFromRepo == null)
            {
                return NotFound();
            }

            var airportToPatch = _mapper.Map<AirportUpdateDto>(airportModelFromRepo.Value);

            patchDoc.ApplyTo(airportToPatch, ModelState);

            if (!TryValidateModel(airportToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(airportToPatch, airportModelFromRepo.Value);
            _repository.UpdateAirport(id, airportModelFromRepo.Value);

            _repository.SaveChanges();

            return NoContent();
        }


        // POST: api/Airports
        [HttpPost]
        public ActionResult<Airport> CreateAirport(AirportCreateDto airportCreateDto)
        {
            var airportModel = _mapper.Map<Airport>(airportCreateDto);
            _repository.CreateAirport(airportModel);
            _repository.SaveChanges();

            var airportReadDto = _mapper.Map<AirportReadDto>(airportModel);

            return CreatedAtRoute(nameof(GetAirport), new { Id = airportReadDto.ID }, airportReadDto);
        }

        // DELETE: api/Airports/5
        [HttpDelete("{id}")]
        public ActionResult<Airport> DeleteAirport(int id)
        {
            var airport = _repository.GetAirportById(id);
            if (airport.Value == null)
            {
                return NotFound();
            }

            _repository.DeleteAirport(airport.Value);
            
            _repository.SaveChanges();

            return Ok(_mapper.Map<AirportReadDto>(airport.Value));
        }
    }
}
