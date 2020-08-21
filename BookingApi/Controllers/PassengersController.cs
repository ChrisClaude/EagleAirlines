using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookingApi.Data.Repository.PassengerRepo;
using BookingApi.Data.Repository.PassengerRepo;
using BookingApi.Data.Util;
using BookingApi.Dtos.PassengerDto;
using BookingApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengersController : ControllerBase
    {
        private readonly IPassengerRepo _repository;
        private readonly IMapper _mapper;

        public PassengersController(IPassengerRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        // GET: api/Passengers?search=value&sort=value?pageIndex=3&pageSize=35
        /// <summary>
        /// Get all passengers from the database. 
        /// </summary>
        /// <param name="parameters">this represent the set of query string parameters which in this case are
        ///    search for searching passengers, sort for sorting, pageIndex for page number of the paged data, pageSize to specify
        ///     the number of returned elements
        /// </param>
        /// <returns>An array of airport objects</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Passenger>>> GetAllPassengers([FromQuery] PassengerQueryParameters parameters)
        {            
            var passengers = await _repository.GetAllAsync(parameters);
                        
            var metadata = new 
            {
                ((PaginatedList<Passenger>) passengers).ItemCount,
                ((PaginatedList<Passenger>) passengers).PageSize,
                ((PaginatedList<Passenger>) passengers).PageIndex,
                ((PaginatedList<Passenger>) passengers).TotalPages,
                ((PaginatedList<Passenger>) passengers).HasNextPage,
                ((PaginatedList<Passenger>) passengers).HasPreviousPage
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(_mapper.Map<IEnumerable<PassengerReadDto>>(passengers));
        }
        
        // PUT: api/Passengers/5
        /// <summary>
        /// Updates an airport object
        /// </summary>
        /// <param name="id">the id of the airport to update</param>
        /// <param name="airportUpdateDto">the updated object</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePassengerAsync(int id, PassengerUpdateDto airportUpdateDto)
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
            //_repository.UpdatePassenger(airportFromRepo.Value); // this is achieved by the previous code

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
        public async Task<ActionResult> PartialPassengerUpdateAsync(int id, JsonPatchDocument<PassengerUpdateDto> patchDoc)
        {
            var airportModelFromRepo = await _repository.GetByIdAsync(id);
            if (airportModelFromRepo == null)
            {
                return NotFound();
            }

            var airportToPatch = _mapper.Map<PassengerUpdateDto>(airportModelFromRepo);

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
        
        // POST: api/Passengers
        /// <summary>
        /// Creates an airport 
        /// </summary>
        /// <param name="airportCreateDto">The airport object to create.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Passenger>> CreatePassengerAsync(PassengerCreateDto airportCreateDto)
        {
            var airportModel = _mapper.Map<Passenger>(airportCreateDto);
            await _repository.CreateAsync(airportModel);
            await _repository.SaveChangesAsync();

            var airportReadDto = _mapper.Map<PassengerReadDto>(airportModel);

            return CreatedAtRoute(nameof(GetPassengerAsync), new { Id = airportReadDto.ID }, airportReadDto);
        }
        
        // DELETE: api/Passengers/5
        /// <summary>
        /// Deletes an airport
        /// </summary>
        /// <param name="id">id of the object to be deleted</param>
        /// <returns>the deleted object</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Passenger>> DeletePassengerAsync(int id)
        {
            var airport = await _repository.GetByIdAsync(id);
            if (airport == null)
            {
                return NotFound();
            }

            _repository.Delete(airport);

            await _repository.SaveChangesAsync();

            return Ok(_mapper.Map<PassengerReadDto>(airport));
        }


    }
    
}