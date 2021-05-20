using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecurePrivacyAPI.Models;
using SecurePrivacyAPI.Services;
using System.Threading.Tasks;

namespace SecurePrivacyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly ILogger<PeopleController> _logger;
        private readonly IPersonService _personService;

        public PeopleController(ILogger<PeopleController> logger, IPersonService service)
        {
            _logger = logger;
            _personService = service;
        }

        [HttpPost]
        public async Task<ActionResult<Person>> Create(PersonDto person)
        {
            return Ok(await _personService.Create(person));
        }

        /// <summary>
        /// Endpoint to get all people
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Person>> Get()
        {
            var value = await _personService.GetAll();
            return Ok(value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetById(string id)
        {
            var person = await _personService.Get(id);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, PersonDto model)
        {
            var result = await _personService.Update(id, model);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(new { message = "Updated Successfully", result });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var person = await _personService.Get(id);
            if (person == null)
            {
                return NotFound();
            }

            await _personService.Remove(person.Id);
            return Ok(new { message = "Deleted Successfully" });
        }
    }
}
