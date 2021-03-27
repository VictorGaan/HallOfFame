using System.Threading.Tasks;

namespace Interfaces.ApiHallOfFame
{
    public interface IRepositoryManager
    {
        IPerson Person { get; }
        Task Save();
    }
}
