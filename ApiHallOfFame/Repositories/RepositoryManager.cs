using ApiHallOfFame;
using Interfaces.ApiHallOfFame;
using System.Threading.Tasks;

namespace Repositories.ApiHallOfFame
{
    public class RepositoryManager: IRepositoryManager
    {
        private IPerson PersonRepository;
        private HallOfFameContext Context;
        public RepositoryManager(HallOfFameContext context)
        {
            Context = context;
        }
        public IPerson Person
        {
            get
            {
                if (PersonRepository==null)
                {
                    PersonRepository = new PersonRepository(Context);
                }
                return PersonRepository;
            }
        }
        public Task Save() => Context.SaveChangesAsync();
    }
}
