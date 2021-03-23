using ApiHallOfFame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly HallOfFameContext context;

        public PersonController(HallOfFameContext context)
        {
            this.context = context;
        }
        [Route("Persons")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons() => await context.Persons.Include(x => x.Skills).ToListAsync();

        [Route("Person/{id}")]
        [HttpGet]
        public async Task<ActionResult<Person>> GetPerson(long id)
        {
            var person = await context.Persons.Include(x => x.Skills).FirstOrDefaultAsync(x => x.Id == id);

            if (person == null)
            {
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
            if (!PersonExists(id))
            {
                return NotFound();
            }
            context.Entry(person).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }
        [Route("Person")]
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            context.Persons.Add(person);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }
        [Route("Person/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePerson(long id)
        {
            var person = await context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            if (person.Skills != null)
            {
                return BadRequest();
            }
            context.Persons.Remove(person);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(long id)
        {
            return context.Persons.Any(e => e.Id == id);
        }
    }
}
