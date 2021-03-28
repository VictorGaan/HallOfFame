using ApiHallOfFame.Interfaces;
using ApiHallOfFame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiHallOfFame.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;
        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }
        [Route("Persons")]
        [HttpGet]
        public async Task<IEnumerable<Person>> GetPersons() => await _personService.GetPersonsAsync();

        [Route("Person/{id}")]
        [HttpGet]
        public async Task<ActionResult<Person>> GetPerson(long id) => await _personService.GetPersonAsync(id);

        [Route("Person/{id}")]
        [HttpPut]
        public async Task<IActionResult> PutPerson(long id, Person person) => await _personService.UpdateAsync(id, person);

        [Route("Person")]
        [HttpPost]
        public async Task<IActionResult> PostPerson(Person person)=> await _personService.AddAsync(person);

        [Route("Person/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePerson(long id) =>await _personService.DeleteAsync(id);
    }
}
