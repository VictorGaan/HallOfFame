using ApiHallOfFame.Models;
using Interfaces.ApiHallOfFame;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiHallOfFame.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IRepositoryManager Repository;
        private readonly ILogger<PersonController> Logger;

        public PersonController(IRepositoryManager repository, ILogger<PersonController> logger)
        {
            Repository = repository;
            Logger = logger;
        }
        [Route("Persons")]
        [HttpGet]
        public async Task<IEnumerable<Person>> GetPersons() => await Repository.Person.GetPersons();

        [Route("Person/{id}")]
        [HttpGet]
        public async Task<ActionResult<Person>> GetPerson(long id)
        {
            var person = await Repository.Person.GetPerson(id);

            if (person == null)
            {
                Logger.LogError("Not found, this person");
                return NotFound();
            }

            return person;
        }
        [Route("Person/{id}")]
        [HttpPut]
        public async Task<IActionResult> PutPerson(long id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }
            Repository.Person.UpdatePerson(person);
            await Repository.Save();
            return Ok();
        }
        [Route("Person")]
        [HttpPost]
        public async Task<IActionResult> PostPerson(Person person)
        {
            Repository.Person.CreatePerson(person);
            await Repository.Save();
            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }
        [Route("Person/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePerson(long id)
        {
            var person = await Repository.Person.GetPerson(id);
            if (person == null)
            {
                Logger.LogError("Id does not match");
                return NotFound();
            }
            Repository.Person.DeletePerson(person);
            await Repository.Save();
            return Ok();
        }
    }
}
