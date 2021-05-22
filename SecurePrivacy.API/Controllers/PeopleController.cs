using Microsoft.AspNetCore.Mvc;
using SecurePrivacy.API.Models;
using SecurePrivacy.API.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurePrivacy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PeopleController(IPersonService service)
        {
            _personService = service;
        }

        /// <summary>
        /// Creates a new Person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Person> Create(PersonDto person)
        {
            return await _personService.Create(person);
        }

        /// <summary>
        /// Gets all People
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<Person>> Get()
        {
            var value = await _personService.GetAll();
            return value.ToList();
        }

        /// <summary>
        /// Used to get details of a particular person
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetById(string id)
        {
            var person = await _personService.Get(id);
            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        /// <summary>
        /// Used to update a Person's information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<Person>> Update(string id, PersonDto model)
        {
            var result = await _personService.Update(id, model);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        /// <summary>
        /// Removes a Person from the System
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Person>> Delete(string id)
        {
            var person = await _personService.Get(id);
            if (person == null)
            {
                return NotFound();
            }

            await _personService.Remove(person.Id);
            return Ok(new { message = "deleted successfully" });
        }

        /// <summary>
        /// Gets a list of the number of People in each category
        /// </summary>
        /// <returns></returns>
        [HttpGet("stats")]
        public async Task<List<PersonGroup>> GetStats()
        {
            var result = await _personService.Stats();
            return result;
        }
    }
}
