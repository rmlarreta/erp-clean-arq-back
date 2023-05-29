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

        public bool Any(Expression<Func<T, bool>> expression) => _erpbdContext.Set<T>().Any(expression);

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression) => await _erpbdContext.Set<T>().AnyAsync(expression);

        public void Add(T entity)
        {
            _erpbdContext.Set<T>().Add(entity);
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

        public async Task<T> Get(Guid id)
        {
            return await _erpbdContext.Set<T>().FindAsync(id);
        }
        public async Task<T> Get(Expression<Func<T, bool>> expression, Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _erpbdContext.Set<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.FirstOrDefaultAsync(expression)!;
        }
        public async Task<T> Get(Guid id, Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _erpbdContext.Set<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public DbSet<T> GetAll()
        {
            return _erpbdContext.Set<T>();
        }

        public void Update(T entity)
        {
            _erpbdContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task<T> GetReloadAsync(Guid id)
        {
            var changedEntries = _erpbdContext.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();
            foreach (var entry in changedEntries)
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