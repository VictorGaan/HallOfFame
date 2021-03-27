using ApiHallOfFame;
using Interfaces.ApiHallOfFame;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Repositories.ApiHallOfFame
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected HallOfFameContext Context;
        public RepositoryBase(HallOfFameContext context)
        {
            Context = context;
        }
        public void Create(T entity) => Context.Set<T>().Add(entity);

        public void Delete(T entity) => Context.Set<T>().Remove(entity);

        public IQueryable<T> FindAll(params Expression<Func<T, object>>[] includes)
        {
            var query = Context.Set<T>().AsQueryable();
            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
        public IQueryable<T> FindAll()
        {
            return Context.Set<T>().AsQueryable();
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public void Update(T entity) => Context.Set<T>().Update(entity);

    }
}
