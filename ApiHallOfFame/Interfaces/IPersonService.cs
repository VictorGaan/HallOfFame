using ApiHallOfFame.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiHallOfFame.Interfaces
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetPersonsAsync();
        Task<ActionResult<Person>> GetPersonAsync(long id);
        Task<IActionResult> AddAsync(Person person);
        Task<IActionResult> UpdateAsync(long id, Person person);
        Task<IActionResult> DeleteAsync(long id);
    }
}
