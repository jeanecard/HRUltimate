using Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository
{
    internal class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext;
        public RepositoryBase(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Create(T entity)
        {
            RepositoryContext.Set<T>().Add(entity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            RepositoryContext.Set<T>().Remove(entity);
        }
        /// <summary>
        /// 
        /// </summary>
        public void DeleteAll()
        {
            RepositoryContext.Set<T>().RemoveRange(RepositoryContext.Set<T>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        public IQueryable<T> FindAll(bool trackChanges)
        {
            return !trackChanges ?
            RepositoryContext.Set<T>().AsNoTracking() 
            :
            RepositoryContext.Set<T>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return !trackChanges ?
            RepositoryContext.Set<T>()
            .Where(expression)
            .AsNoTracking() 
            :
            RepositoryContext.Set<T>()
            .Where(expression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            RepositoryContext.Set<T>().Update(entity);
        }
    }
}
