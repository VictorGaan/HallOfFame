using ApiHallOfFame.Interfaces;
using System.Threading.Tasks;

namespace ApiHallOfFame.Repositories
{
    public abstract class BaseRepository : IBaseRepository
    {
        protected readonly HallOfFameContext _context;

        public BaseRepository(HallOfFameContext context)
        {
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
