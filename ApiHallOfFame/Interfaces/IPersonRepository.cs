using ApiHallOfFame.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiHallOfFame.Interfaces
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> ListAsync();
        Task AddAsync(Person person);
        Task<Person> FindByIdAsync(long id);
        Task Update(Person person);
        Task Remove(Person person);
    }
}
