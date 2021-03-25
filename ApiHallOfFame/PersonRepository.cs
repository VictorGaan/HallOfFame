using ApiHallOfFame;
using ApiHallOfFame.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiHallOfFame
{
    public class PersonRepository : IPerson
    {
        private HallOfFameContext Context;
        public PersonRepository(HallOfFameContext context)
        {
            Context = context;
        }
        public void DeletePerson(Person person)
        {
            Context.Persons.Remove(person);
        }

        public async Task<Person> GetPerson(long id)
        {
            return await Context.Persons.Include(x => x.Skills).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Person>> GetPersons()
        {
            return await Context.Persons.Include(x => x.Skills).ToListAsync();
        }

        public void InsertPerson(Person person)
        {
            Context.Persons.Add(person);
        }

        public async Task Save()
        {
            await Context.SaveChangesAsync();
        }

        public void UpdatePerson(Person person)
        {
            Context.Entry(person).State = EntityState.Modified;
            foreach (var skill in person.Skills.ToList())
            {
                Context.Entry(skill).State = EntityState.Modified;
            }
        }
    }
}
