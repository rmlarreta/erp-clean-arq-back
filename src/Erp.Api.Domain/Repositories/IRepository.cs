using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Erp.Api.Domain.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        bool Any(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task<T> Get(Guid id);
        Task<T> Get(Expression<Func<T, bool>> expression, Expression<Func<T, object>>[] includeProperties);
        Task<T> Get(Guid id, Expression<Func<T, object>>[] includeProperties);
        DbSet<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> GetAll(Expression<Func<T, bool>> expression, Expression<Func<T, object>>[] includeProperties);
        void Add(T entity);
        void AddRange(List<T> entities);
        void Update(T entity);
        void UpdateRange(List<T> entities);
        void Delete(T entity);
        void DeleteRange(List<T> entities);
        Task<T> GetReloadAsync(Guid id);
    }
}
