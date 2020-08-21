using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookingApi.Data.Repository.PassengerRepo;
using BookingApi.Data.Util;
using BookingApi.Dtos.PassengerDto;
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
        /// <returns>An array of passenger objects</returns>
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
        
        // GET: api/Passengers/5
        /// <summary>
        /// Get a passenger by its id
        /// </summary>
        /// <param name="id">the id of the passenger requested</param>
        /// <returns>An passenger object</returns>
        [HttpGet("{id:int}", Name = "GetPassenger")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Passenger>> GetPassengerAsync(int id)
        {
            var passenger = await _repository.GetByIdAsync(id);

            if (passenger == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PassengerReadDto>(passenger));
        }
        
        // PUT: api/Passengers/5
        /// <summary>
        /// Updates an passenger object
        /// </summary>
        /// <param name="id">the id of the passenger to update</param>
        /// <param name="passengerUpdateDto">the updated object</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePassengerAsync(int id, PassengerUpdateDto passengerUpdateDto)
        {
            if (id != passengerUpdateDto.Id)
            {
                return BadRequest();
            }

            var passengerFromRepo = await _repository.GetByIdAsync(id);

            if (passengerFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(passengerUpdateDto, passengerFromRepo);
            //_repository.UpdatePassenger(passengerFromRepo.Value); // this is achieved by the previous code

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
        /// partially updates an passenger
        /// </summary>
        /// <param name="id">the id of the passenger to update</param>
        /// <param name="patchDoc">the json object with the specific attribute to be updated</param>
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PartialPassengerUpdateAsync(int id, JsonPatchDocument<PassengerUpdateDto> patchDoc)
        {
            var passengerModelFromRepo = await _repository.GetByIdAsync(id);
            if (passengerModelFromRepo == null)
            {
                return NotFound();
            }

            var passengerToPatch = _mapper.Map<PassengerUpdateDto>(passengerModelFromRepo);

            patchDoc.ApplyTo(passengerToPatch, ModelState);

            if (!TryValidateModel(passengerToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(passengerToPatch, passengerModelFromRepo);
            _repository.Update(passengerModelFromRepo);

            await _repository.SaveChangesAsync();

            return NoContent();
        }
        
        // POST: api/Passengers
        /// <summary>
        /// Creates an passenger 
        /// </summary>
        /// <param name="passengerCreateDto">The passenger object to create.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Passenger>> CreatePassengerAsync(PassengerCreateDto passengerCreateDto)
        {
            var passengerModel = _mapper.Map<Passenger>(passengerCreateDto);
            await _repository.CreateAsync(passengerModel);
            await _repository.SaveChangesAsync();

            var passengerReadDto = _mapper.Map<PassengerReadDto>(passengerModel);

            return CreatedAtRoute(nameof(GetPassengerAsync), new { Id = passengerReadDto.Id }, passengerReadDto);
        }
        
        // DELETE: api/Passengers/5
        /// <summary>
        /// Deletes an passenger
        /// </summary>
        /// <param name="id">id of the object to be deleted</param>
        /// <returns>the deleted object</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Passenger>> DeletePassengerAsync(int id)
        {
            var passenger = await _repository.GetByIdAsync(id);
            if (passenger == null)
            {
                return NotFound();
            }

            _repository.Delete(passenger);

            await _repository.SaveChangesAsync();

            return Ok(_mapper.Map<PassengerReadDto>(passenger));
        }


    }
    
}