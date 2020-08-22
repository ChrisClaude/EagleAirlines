using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookingApi.Data.Repository.CustomerRepo;
using BookingApi.Data.Util;
using BookingApi.Dtos.CustomerDto;
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
    public class CustomersController : ControllerBase
    {
    
        private readonly ICustomerRepo _repository;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Customers?search=France&sort=name?pageIndex=3&pageSize=35
        /// <summary>
        /// Get all customers from the database. 
        /// </summary>
        /// <param name="parameters">this represents the search, sort, pageIndex, and pageSize query string parameters</param>
        /// <returns>An array of customer objects</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers(
            [FromQuery] CustomerQueryParameters parameters)
        {
            var customers = await _repository.GetAllAsync(parameters);

            var metadata = new
            {
                ((PaginatedList<Customer>) customers).ItemCount,
                parameters.PageSize,
                ((PaginatedList<Customer>) customers).PageIndex,
                ((PaginatedList<Customer>) customers).TotalPages,
                ((PaginatedList<Customer>) customers).HasNextPage,
                ((PaginatedList<Customer>) customers).HasPreviousPage
            };


            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(_mapper.Map<IEnumerable<CustomerReadDto>>(customers));
        }

        // GET: api/Customers/5
        /// <summary>
        /// Get an customer by its id
        /// </summary>
        /// <param name="id">the id of the customer requested</param>
        /// <returns>An customer object</returns>
        [HttpGet("{id:int}", Name = "GetCustomerAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Customer>> GetCustomerAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CustomerReadDto>(customer));
        }

        // PUT: api/Customers/5
        /// <summary>
        /// Updates an customer object
        /// </summary>
        /// <param name="id">the id of the customer to update</param>
        /// <param name="customerUpdateDto">the updated object</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateCustomerAsync(int id, CustomerUpdateDto customerUpdateDto)
        {
            if (id != customerUpdateDto.Id)
            {
                return BadRequest();
            }

            var customerFromRepo = await _repository.GetByIdAsync(id);

            if (customerFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(customerUpdateDto, customerFromRepo);
            //_repository.UpdateCustomer(customerFromRepo.Value); // this is achieved by the previous code

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
        /// partially updates an customer
        /// </summary>
        /// <param name="id">the id of the customer to update</param>
        /// <param name="patchDoc">the json object with the specific attribute to be updated</param>
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PartialCustomerUpdateAsync(int id,
            JsonPatchDocument<CustomerUpdateDto> patchDoc)
        {
            var customerModelFromRepo = await _repository.GetByIdAsync(id);
            if (customerModelFromRepo == null)
            {
                return NotFound();
            }

            var customerToPatch = _mapper.Map<CustomerUpdateDto>(customerModelFromRepo);

            patchDoc.ApplyTo(customerToPatch, ModelState);

            if (!TryValidateModel(customerToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(customerToPatch, customerModelFromRepo);
            _repository.Update(customerModelFromRepo);

            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Customers
        /// <summary>
        /// Creates an customer 
        /// </summary>
        /// <param name="customerCreateDto">The customer object to create.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Customer>> CreateCustomerAsync(CustomerCreateDto customerCreateDto)
        {
            var customerModel = _mapper.Map<Customer>(customerCreateDto);
            
            await _repository.CreateAsync(customerModel);

            await _repository.SaveChangesAsync();

            // here we query the customer that we just created in order to load its navigation property
            customerModel = await _repository.GetByIdAsync(customerModel.Id);
            var customerReadDto = _mapper.Map<CustomerReadDto>(customerModel);

            return CreatedAtRoute(nameof(GetCustomerAsync), new {customerReadDto.Id}, customerReadDto);
        }

        // DELETE: api/Customers/5
        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <param name="id">id of the object to be deleted</param>
        /// <returns>the deleted object</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Customer>> DeleteCustomerAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _repository.Delete(customer);

            await _repository.SaveChangesAsync();

            return Ok(_mapper.Map<CustomerReadDto>(customer));
        }
    

    }
}