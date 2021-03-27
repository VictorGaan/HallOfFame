using ApiHallOfFame;
using ApiHallOfFame.Models;
using Interfaces.ApiHallOfFame;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.ApiHallOfFame
{
    public class PersonRepository : RepositoryBase<Person>, IPerson
    {
        public PersonRepository(HallOfFameContext context):base(context)
        {
        }

        public void CreatePerson(Person person)
        {
            Create(person);
        }

        public void DeletePerson(Person person)
        {
            Delete(person);
        }

        public async Task<Person> GetPerson(long id)
        {
            return await FindAll(x=>x.Skills).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Person>> GetPersons()
        {
            return await FindAll(x=>x.Skills).ToListAsync();
        }

        public void UpdatePerson(Person person)
        {
            Update(person);
        }
    }
}
