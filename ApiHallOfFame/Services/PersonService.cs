using ApiHallOfFame.Interfaces;
using ApiHallOfFame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiHallOfFame.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PersonService> _logger;
        public PersonService(IPersonRepository personRepository, ILogger<PersonService> logger)
        {
            _personRepository = personRepository;
            _logger = logger;
        }

        public async Task<IActionResult> AddAsync(Person person)
        {
            await _personRepository.AddAsync(person);
            return new CreatedAtActionResult("GetPerson", "Person", new { id = person.Id }, person);
        }

        public async Task<IActionResult> DeleteAsync(long id)
        {
            var person = await _personRepository.FindByIdAsync(id);
            if (person == null)
            {
                _logger.LogError("Person not found.");
                return new NotFoundResult();
            }
            await _personRepository.Remove(person);
            return new OkResult();
        }

        public async Task<ActionResult<Person>> GetPersonAsync(long id)
        {
            var person = await _personRepository.FindByIdAsync(id);
            if (person == null)
            {
                _logger.LogError("Person not found.");
                return new NotFoundResult();
            }
            return person;
        }

        public async Task<IEnumerable<Person>> GetPersonsAsync()
        {
            return await _personRepository.ListAsync();
        }

        public async Task<IActionResult> UpdateAsync(long id, Person person)
        {
            if (id != person.Id)
            {
                _logger.LogError("IDs do not match.");
                return new BadRequestResult();
            }
            await _personRepository.Update(person);
            return new OkResult();
        }
    }
}
