using Erp.Api.Domain.Repositories;
using System.Linq.Expressions;

namespace Erp.Api.Domain.Services
{
    public interface IService<TEntity> where TEntity : class, IEntity
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity> Get(Guid id);
        Task<TEntity> Get(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> Get(Guid id, Expression<Func<TEntity, object>>[] includeProperties);
        Task<int> Add(TEntity data);
        Task<int> Add(List<TEntity> data);
        Task<int> Delete(Guid id);
        Task<int> Delete(TEntity data);
        Task<int> Delete(List<TEntity> data);
        Task<int> Update(TEntity data);
        Task<int> UpdateRange(List<TEntity> data);
        Task<bool> Exist(Guid id);
        bool Any(Expression<Func<TEntity, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);
    }
}
