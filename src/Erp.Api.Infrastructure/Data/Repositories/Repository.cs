using Erp.Api.Domain.Repositories;
using Erp.Api.Infraestructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Erp.Api.Infrastructure.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly ErpDbContext _erpbdContext;

        public Repository(ErpDbContext erpbdContext)
        {
            _erpbdContext = erpbdContext;
        }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            return _erpbdContext.Set<T>().Any(expression);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _erpbdContext.Set<T>().AnyAsync(expression);
        }

        public void Add(T entity)
        {
            _erpbdContext.Set<T>().Add(entity);
        }

        public void AddRange(List<T> entities)
        {
            _erpbdContext.AddRange(entities);
        }

        public void Delete(T entity)
        {
            if (_erpbdContext.Entry(entity).State == EntityState.Detached)
            {
                _erpbdContext.Set<T>().Attach(entity);
            }
            _erpbdContext.Entry(entity).State = EntityState.Deleted;
            _erpbdContext.Set<T>().Remove(entity);
        }

        public void DeleteRange(List<T> entities)
        {
            if (_erpbdContext.Entry(entities).State == EntityState.Detached)
            {
                _erpbdContext.Set<T>().AttachRange(entities);
            }
            _erpbdContext.Entry(entities).State = EntityState.Deleted;
            _erpbdContext.Set<T>().RemoveRange(entities);
        }

        public async Task<T> Get(Guid id)
        {
            return await _erpbdContext.Set<T>().FindAsync(id);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _erpbdContext.Set<T>();

            foreach (Expression<Func<T, object>>? includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.OrderBy(x => x.Id).FirstOrDefaultAsync(expression)!;
        }

        public async Task<T> Get(Guid id, Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _erpbdContext.Set<T>();

            foreach (Expression<Func<T, object>>? includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.Id == id);
        }

        public DbSet<T> GetAll()
        {
            return _erpbdContext.Set<T>();
        }

        public IQueryable<T> GetAll(Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _erpbdContext.Set<T>();

            foreach (Expression<Func<T, object>>? includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> expression, Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _erpbdContext.Set<T>();

            if (includeProperties != null)
            {
                foreach (Expression<Func<T, object>>? includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (expression != null)
            {
                query = query.Where(expression);
            }

            return query;
        }

        public void Update(T entity)
        {
            _erpbdContext.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateRange(List<T> entities)
        {
            _erpbdContext.UpdateRange(entities);
        }

        public async Task<T> GetReloadAsync(Guid id)
        {
            List<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry>? changedEntries = _erpbdContext.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry? entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
            return await _erpbdContext.Set<T>().FindAsync(id);
        }
    }
}