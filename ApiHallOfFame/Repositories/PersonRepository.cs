using ApiHallOfFame.Interfaces;
using ApiHallOfFame.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiHallOfFame.Repositories
{
    public class PersonRepository : BaseRepository, IPersonRepository
    {
        public PersonRepository(HallOfFameContext context) : base(context) { }

        public async Task AddAsync(Person person)
        {
            await _context.Persons.AddAsync(person);
            await CompleteAsync();
        }

        public async Task<Person> FindByIdAsync(long id)
        {
            return await _context.Persons.Include(x => x.Skills).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Person>> ListAsync()
        {
            return await _context.Persons.Include(x => x.Skills).AsNoTracking().ToListAsync();
        }

        public async Task Remove(Person person)
        {
            _context.Persons.Remove(person);
            await CompleteAsync();
        }

        public async Task Update(Person person)
        {
            _context.Persons.Update(person);
            await CompleteAsync();
        }

    }
}
