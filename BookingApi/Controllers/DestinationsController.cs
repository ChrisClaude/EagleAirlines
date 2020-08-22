using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookingApi.Data.Repository.DestinationRepo;
using BookingApi.Data.Util;
using BookingApi.Dtos.DestinationDto;
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
    public class DestinationsController : ControllerBase
    {
        private readonly IDestinationRepo _repository;
        private readonly IMapper _mapper;

        public DestinationsController(IDestinationRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Destinations?search=France&sort=name?pageIndex=3&pageSize=35
        /// <summary>
        /// Get all destinations from the database. 
        /// </summary>
        /// <param name="parameters">this represents the search, sort, pageIndex, and pageSize query string parameters</param>
        /// <returns>An array of destination objects</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Destination>>> GetAllDestinations([FromQuery] DestinationQueryParameters parameters)
        {
            var destinations = await _repository.GetAllAsync(parameters);
                        
            var metadata = new 
            {
                ((PaginatedList<Destination>) destinations).ItemCount,
                ((PaginatedList<Destination>) destinations).PageSize,
                ((PaginatedList<Destination>) destinations).PageIndex,
                ((PaginatedList<Destination>) destinations).TotalPages,
                ((PaginatedList<Destination>) destinations).HasNextPage,
                ((PaginatedList<Destination>) destinations).HasPreviousPage
            };


            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(_mapper.Map<IEnumerable<DestinationReadDto>>(destinations));
        }

        // GET: api/Destinations/5
        /// <summary>
        /// Get an destination by its id
        /// </summary>
        /// <param name="id">the id of the destination requested</param>
        /// <returns>An destination object</returns>
        [HttpGet("{id:int}", Name = "GetDestinationAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Destination>> GetDestinationAsync(int id)
        {
            var destination = await _repository.GetByIdAsync(id);

            if (destination == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<DestinationReadDto>(destination));
        }

        // PUT: api/Destinations/5
        /// <summary>
        /// Updates an destination object
        /// </summary>
        /// <param name="id">the id of the destination to update</param>
        /// <param name="destinationUpdateDto">the updated object</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateDestinationAsync(int id, DestinationUpdateDto destinationUpdateDto)
        {
            if (id != destinationUpdateDto.Id)
            {
                return BadRequest();
            }

            var destinationFromRepo = await _repository.GetByIdAsync(id);

            if (destinationFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(destinationUpdateDto, destinationFromRepo);
            //_repository.UpdateDestination(destinationFromRepo.Value); // this is achieved by the previous code

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
        /// partially updates an destination
        /// </summary>
        /// <param name="id">the id of the destination to update</param>
        /// <param name="patchDoc">the json object with the specific attribute to be updated</param>
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PartialDestinationUpdateAsync(int id, JsonPatchDocument<DestinationUpdateDto> patchDoc)
        {
            var destinationModelFromRepo = await _repository.GetByIdAsync(id);
            if (destinationModelFromRepo == null)
            {
                return NotFound();
            }

            var destinationToPatch = _mapper.Map<DestinationUpdateDto>(destinationModelFromRepo);

            patchDoc.ApplyTo(destinationToPatch, ModelState);

            if (!TryValidateModel(destinationToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(destinationToPatch, destinationModelFromRepo);
            _repository.Update(destinationModelFromRepo);

            await _repository.SaveChangesAsync();

            return NoContent();
        }
        
        // POST: api/Destinations
        /// <summary>
        /// Creates an destination 
        /// </summary>
        /// <param name="destinationCreateDto">The destination object to create.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Destination>> CreateDestinationAsync(DestinationCreateDto destinationCreateDto)
        {
            var destinationModel = _mapper.Map<Destination>(destinationCreateDto);
            
            var isFlightIdUnique = await ((DestinationRepo) _repository).IsFlightIdUnique(destinationModel.FlightId);
            if (!isFlightIdUnique) 
            {
                // the flightId isn't unique
                return BadRequest();
            }
            
            await _repository.CreateAsync(destinationModel);
            
            await _repository.SaveChangesAsync();

            // here we query the departure that we just created in order to load its navigation property
            destinationModel = await _repository.GetByIdAsync(destinationModel.Id);
            var destinationReadDto = _mapper.Map<DestinationReadDto>(destinationModel);

            return CreatedAtRoute(nameof(GetDestinationAsync), new { Id = destinationReadDto.Id }, destinationReadDto);
        }
        
        // DELETE: api/Destinations/5
        /// <summary>
        /// Deletes an destination
        /// </summary>
        /// <param name="id">id of the object to be deleted</param>
        /// <returns>the deleted object</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Destination>> DeleteDestinationAsync(int id)
        {
            var destination = await _repository.GetByIdAsync(id);
            if (destination == null)
            {
                return NotFound();
            }

            _repository.Delete(destination);

            await _repository.SaveChangesAsync();

            return Ok(_mapper.Map<DestinationReadDto>(destination));
        }

    }
}