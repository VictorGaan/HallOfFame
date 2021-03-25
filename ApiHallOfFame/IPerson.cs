using ApiHallOfFame.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiHallOfFame
{
    public interface IPerson
    {
        Task<IEnumerable<Person>> GetPersons();
        Task<Person> GetPerson(long id);
        void InsertPerson(Person person);
        void DeletePerson(Person person);
        void UpdatePerson(Person person);
        Task Save();
    }
}
