using System;
using System.Linq;
using System.Linq.Expressions;

namespace Interfaces.ApiHallOfFame
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includes);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
