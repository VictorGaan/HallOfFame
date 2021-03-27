using ApiHallOfFame.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces.ApiHallOfFame
{
    public interface IPerson
    {
        Task<IEnumerable<Person>> GetPersons();
        Task<Person> GetPerson(long id);
        void CreatePerson(Person person);
        void DeletePerson(Person person);
        void UpdatePerson(Person person);
    }
}
