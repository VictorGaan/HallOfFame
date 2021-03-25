using ApiHallOfFame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IPerson Context;
        private readonly ILogger<PersonController> Logger;

        public PersonController(IPerson context, ILogger<PersonController> logger)
        {
            Context = context;
            Logger = logger;
        }
        [Route("Persons")]
        [HttpGet]
        public async Task<IEnumerable<Person>> GetPersons() => await Context.GetPersons();

        [Route("Person/{id}")]
        public async Task<ActionResult<Person>> GetPerson(long id)
        {
            var person = await Context.GetPerson(id);

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
            Context.UpdatePerson(person);
            await Context.Save();
            return Ok();
        }
        [Route("Person")]
        [HttpPost]
        public async Task<IActionResult> PostPerson(Person person)
        {
            Context.InsertPerson(person);
            await Context.Save();
            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }
        [Route("Person/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePerson(long id)
        {
            var person = await Context.GetPerson(id);
            if (person == null)
            {
                Logger.LogError("Id does not match");
                return NotFound();
            }
            Context.DeletePerson(person);
            await Context.Save();
            return Ok();
        }
    }
}
