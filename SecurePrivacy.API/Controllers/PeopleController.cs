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

        [HttpPost]
        public async Task<Person> Create(PersonDto person)
        {
            return await _personService.Create(person);
        }

        /// <summary>
        /// Endpoint to get all people
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<Person>> Get()
        {
            var value = await _personService.GetAll();
            return value.ToList();
        }

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
    }
}
