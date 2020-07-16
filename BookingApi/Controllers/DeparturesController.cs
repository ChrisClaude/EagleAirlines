using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookingApi.Data.Repository.DepartureRepo;
using BookingApi.Data.Repository.FlightRepo;
using BookingApi.Data.Util;
using BookingApi.Dtos.DepartureDto;
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
    public class DeparturesController : ControllerBase
    {
        private readonly IDepartureRepo _repository;
        private readonly IMapper _mapper;

        public DeparturesController(IDepartureRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Departures?search=France&sort=name?pageIndex=3&pageSize=35
        /// <summary>
        /// Get all departures from the database. 
        /// </summary>
        /// <param name="parameters">this represents the search, sort, pageIndex, and pageSize query string parameters</param>
        /// <returns>An array of departure objects</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Departure>>> GetAllDepartures([FromQuery] DepartureQueryParameters parameters)
        {            
            var departures = await _repository.GetAllAsync(parameters);
                        
            var metadata = new 
            {
                ((PaginatedList<Departure>) departures).ItemCount,
                parameters.PageSize,
                ((PaginatedList<Departure>) departures).PageIndex,
                ((PaginatedList<Departure>) departures).TotalPages,
                ((PaginatedList<Departure>) departures).HasNextPage,
                ((PaginatedList<Departure>) departures).HasPreviousPage
            };


            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(_mapper.Map<IEnumerable<DepartureReadDto>>(departures));
        }

        // GET: api/Departures/5
        /// <summary>
        /// Get an departure by its id
        /// </summary>
        /// <param name="id">the id of the departure requested</param>
        /// <returns>An departure object</returns>
        [HttpGet("{id:int}", Name = "GetDepartureAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Departure>> GetDepartureAsync(int id)
        {
            var departure = await _repository.GetByIdAsync(id);

            if (departure == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<DepartureReadDto>(departure));
        }

        // PUT: api/Departures/5
        /// <summary>
        /// Updates an departure object
        /// </summary>
        /// <param name="id">the id of the departure to update</param>
        /// <param name="departureUpdateDto">the updated object</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateDepartureAsync(int id, DepartureUpdateDto departureUpdateDto)
        {
            if (id != departureUpdateDto.ID)
            {
                return BadRequest();
            }

            var departureFromRepo = await _repository.GetByIdAsync(id);

            if (departureFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(departureUpdateDto, departureFromRepo);
            //_repository.UpdateDeparture(departureFromRepo.Value); // this is achieved by the previous code

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
        /// partially updates an departure
        /// </summary>
        /// <param name="id">the id of the departure to update</param>
        /// <param name="patchDoc">the json object with the specific attribute to be updated</param>
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PartialDepartureUpdateAsync(int id, JsonPatchDocument<DepartureUpdateDto> patchDoc)
        {
            var departureModelFromRepo = await _repository.GetByIdAsync(id);
            if (departureModelFromRepo == null)
            {
                return NotFound();
            }

            var departureToPatch = _mapper.Map<DepartureUpdateDto>(departureModelFromRepo);

            patchDoc.ApplyTo(departureToPatch, ModelState);

            if (!TryValidateModel(departureToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(departureToPatch, departureModelFromRepo);
            _repository.Update(departureModelFromRepo);

            await _repository.SaveChangesAsync();

            return NoContent();
        }
        
        // POST: api/Departures
        /// <summary>
        /// Creates an departure 
        /// </summary>
        /// <param name="departureCreateDto">The departure object to create.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Departure>> CreateDepartureAsync(DepartureCreateDto departureCreateDto)
        {
            var departureModel = _mapper.Map<Departure>(departureCreateDto);
            await _repository.CreateAsync(departureModel);
            await _repository.SaveChangesAsync();

            var departureReadDto = _mapper.Map<DepartureReadDto>(departureModel);

            return CreatedAtRoute(nameof(GetDepartureAsync), new { Id = departureReadDto.ID }, departureReadDto);
        }
        
        // DELETE: api/Departures/5
        /// <summary>
        /// Deletes an departure
        /// </summary>
        /// <param name="id">id of the object to be deleted</param>
        /// <returns>the deleted object</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Departure>> DeleteDepartureAsync(int id)
        {
            var departure = await _repository.GetByIdAsync(id);
            if (departure == null)
            {
                return NotFound();
            }

            _repository.Delete(departure);

            await _repository.SaveChangesAsync();

            return Ok(_mapper.Map<DepartureReadDto>(departure));
        }

    }
}