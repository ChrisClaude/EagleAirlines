using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookingApi.Data.Repository.SeatRepo;
using BookingApi.Data.Util;
using BookingApi.Dtos.SeatDto;
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
    public class SeatsController : ControllerBase
    {
        private readonly ISeatRepo _repository;
        private readonly IMapper _mapper;

        public SeatsController(ISeatRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Seats?search=France&sort=name?pageIndex=3&pageSize=35
        /// <summary>
        /// Get all seats from the database. 
        /// </summary>
        /// <param name="parameters">this represents the search, sort, pageIndex, and pageSize query string parameters</param>
        /// <returns>An array of flight objects</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Seat>>> GetAllSeats([FromQuery] SeatQueryParameters parameters)
        {
            var seats = await _repository.GetAllAsync(parameters);
                        
            var metadata = new 
            {
                ((PaginatedList<Seat>) seats).ItemCount,
                ((PaginatedList<Seat>) seats).PageSize,
                ((PaginatedList<Seat>) seats).PageIndex,
                ((PaginatedList<Seat>) seats).TotalPages,
                ((PaginatedList<Seat>) seats).HasNextPage,
                ((PaginatedList<Seat>) seats).HasPreviousPage
            };


            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(_mapper.Map<IEnumerable<SeatReadDto>>(seats));
        }

        // GET: api/Seats/5
        /// <summary>
        /// Get an flight by its id
        /// </summary>
        /// <param name="id">the id of the flight requested</param>
        /// <returns>An flight object</returns>
        [HttpGet("{id:int}", Name = "GetSeat")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Seat>> GetSeatAsync(int id)
        {
            var flight = await _repository.GetByIdAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<SeatReadDto>(flight));
        }

        // PUT: api/Seats/5
        /// <summary>
        /// Updates an flight object
        /// </summary>
        /// <param name="id">the id of the flight to update</param>
        /// <param name="flightUpdateDto">the updated object</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateSeatAsync(int id, SeatUpdateDto flightUpdateDto)
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
            //_repository.UpdateSeat(flightFromRepo.Value); // this is achieved by the previous code

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
        public async Task<ActionResult> PartialSeatUpdateAsync(int id, JsonPatchDocument<SeatUpdateDto> patchDoc)
        {
            var flightModelFromRepo = await _repository.GetByIdAsync(id);
            if (flightModelFromRepo == null)
            {
                return NotFound();
            }

            var flightToPatch = _mapper.Map<SeatUpdateDto>(flightModelFromRepo);

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
        
        // POST: api/Seats
        /// <summary>
        /// Creates an flight 
        /// </summary>
        /// <param name="flightCreateDto">The flight object to create.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Seat>> CreateSeatAsync(SeatCreateDto flightCreateDto)
        {
            var flightModel = _mapper.Map<Seat>(flightCreateDto);
            await _repository.CreateAsync(flightModel);
            await _repository.SaveChangesAsync();

            var flightReadDto = _mapper.Map<SeatReadDto>(flightModel);

            return CreatedAtRoute(nameof(GetSeatAsync), new { Id = flightReadDto.ID }, flightReadDto);
        }
        
        // DELETE: api/Seats/5
        /// <summary>
        /// Deletes an flight
        /// </summary>
        /// <param name="id">id of the object to be deleted</param>
        /// <returns>the deleted object</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Seat>> DeleteSeatAsync(int id)
        {
            var flight = await _repository.GetByIdAsync(id);
            if (flight == null)
            {
                return NotFound();
            }

            _repository.Delete(flight);

            await _repository.SaveChangesAsync();

            return Ok(_mapper.Map<SeatReadDto>(flight));
        }

    }
}