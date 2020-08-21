using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookingApi.Data.Repository.BookingRepo;
using BookingApi.Data.Util;
using BookingApi.Dtos.BookingDto;
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
    public class BookingsController : Controller
    {
        private readonly IBookingRepo _repository;
        private readonly IMapper _mapper;

        public BookingsController(IBookingRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Bookings?search=France&sort=name?pageIndex=3&pageSize=35
        /// <summary>
        /// Get all bookings from the database. 
        /// </summary>
        /// <param name="parameters">this represent the set of query string parameters which in this case are
        ///    search for searching bookings, sort for sorting, pageIndex for page number of the paged data, pageSize to specify
        ///     the number of returned elements
        /// </param>
        /// <returns>An array of booking objects</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Booking>>> GetAllBookings([FromQuery] BookingQueryParameters parameters)
        {            
            var bookings = await _repository.GetAllAsync(parameters);
                        
            var metadata = new 
            {
                ((PaginatedList<Booking>) bookings).ItemCount,
                ((PaginatedList<Booking>) bookings).PageSize,
                ((PaginatedList<Booking>) bookings).PageIndex,
                ((PaginatedList<Booking>) bookings).TotalPages,
                ((PaginatedList<Booking>) bookings).HasNextPage,
                ((PaginatedList<Booking>) bookings).HasPreviousPage
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(_mapper.Map<IEnumerable<BookingReadDto>>(bookings));
        }

        // GET: api/Bookings/5
        /// <summary>
        /// Get an booking by its id
        /// </summary>
        /// <param name="id">the id of the booking requested</param>
        /// <returns>An booking object</returns>
        [HttpGet("{id:int}", Name = "GetBooking")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Booking>> GetBookingAsync(Guid id)
        {
            var booking = await _repository.GetByIdAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BookingReadDto>(booking));
        }

        // PUT: api/Bookings/5
        /// <summary>
        /// Updates an booking object
        /// </summary>
        /// <param name="id">the id of the booking to update</param>
        /// <param name="bookingUpdateDto">the updated object</param>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateBookingAsync(Guid id, BookingUpdateDto bookingUpdateDto)
        {
            if (id != bookingUpdateDto.Id)
            {
                return BadRequest();
            }

            var bookingFromRepo = await _repository.GetByIdAsync(id);

            if (bookingFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(bookingUpdateDto, bookingFromRepo);
            //_repository.UpdateBooking(bookingFromRepo.Value); // this is achieved by the previous code

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
        /// partially updates an booking
        /// </summary>
        /// <param name="id">the id of the booking to update</param>
        /// <param name="patchDoc">the json object with the specific attribute to be updated</param>
        [HttpPatch("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PartialBookingUpdateAsync(Guid id, JsonPatchDocument<BookingUpdateDto> patchDoc)
        {
            var bookingModelFromRepo = await _repository.GetByIdAsync(id);
            if (bookingModelFromRepo == null)
            {
                return NotFound();
            }

            var bookingToPatch = _mapper.Map<BookingUpdateDto>(bookingModelFromRepo);

            patchDoc.ApplyTo(bookingToPatch, ModelState);

            if (!TryValidateModel(bookingToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(bookingToPatch, bookingModelFromRepo);
            _repository.Update(bookingModelFromRepo);

            await _repository.SaveChangesAsync();

            return NoContent();
        }
        
        // POST: api/Bookings
        /// <summary>
        /// Creates a booking 
        /// </summary>
        /// <param name="bookingCreateDto">The booking object to create.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Booking>> CreateBookingAsync(BookingCreateDto bookingCreateDto)
        {
            var bookingModel = _mapper.Map<Booking>(bookingCreateDto);
            await _repository.CreateAsync(bookingModel);
            await _repository.SaveChangesAsync();

            var bookingReadDto = _mapper.Map<BookingReadDto>(bookingModel);

            return CreatedAtRoute(nameof(GetBookingAsync), new {bookingReadDto.Id }, bookingReadDto);
        }
        
        // DELETE: api/Bookings/5
        /// <summary>
        /// Deletes an booking
        /// </summary>
        /// <param name="id">id of the object to be deleted</param>
        /// <returns>the deleted object</returns>
        [HttpDelete("{id:string}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Booking>> DeleteBookingAsync(Guid id)
        {
            var booking = await _repository.GetByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _repository.Delete(booking);

            await _repository.SaveChangesAsync();

            return Ok(_mapper.Map<BookingReadDto>(booking));
        }

    }
}