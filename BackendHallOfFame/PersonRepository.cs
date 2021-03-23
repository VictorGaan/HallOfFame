using ApiHallOfFame;
using ApiHallOfFame.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendHallOfFame
{
    public class PersonRepository : IPerson
    {
        private HallOfFameContext context;
        public PersonRepository(HallOfFameContext context)
        {
            this.context = context;
        }
        public void DeletePerson(Person person)
        {
            context.Persons.Remove(person);
        }

        public Person GetPerson(long id)
        {
            return context.Persons.Include(x => x.Skills).FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Person> GetPersons()
        {
            return context.Persons.Include(x => x.Skills).ToList();
        }

        public void InsertPerson(Person person)
        {
            context.Persons.Add(person);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdatePerson(Person person)
        {
            context.Entry(person).State = EntityState.Modified;
        }
    }
}
