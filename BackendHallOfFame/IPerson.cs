using ApiHallOfFame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendHallOfFame
{
    public interface IPerson
    {
        public IEnumerable<Person> GetPersons();
        Person GetPerson(long id);
        void InsertPerson(Person person);
        void DeletePerson(Person person);
        void UpdatePerson(Person person);
        void Save();
    }
}
