using Erp.Api.Domain.Repositories;
using Erp.Api.Infraestructure.DbContexts;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Erp.Api.Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private readonly ErpDbContext _modelContext;

        public UnitOfWork(ErpDbContext modelContext)
        {
            _modelContext = modelContext;
        }

        public ErpDbContext GetContext<T>() where T : class, IEntity
        {
            return GetModelContext();
        }

        public ErpDbContext GetModelContext()
        {
            return _modelContext;
        }

        public IRepository<T> GetRepository<T>() where T : class, IEntity
        {
            return new Repository<T>(GetContext<T>());
        }

        public int SaveChanges<T>() where T : class, IEntity
        {
            return GetContext<T>().SaveChanges();
        }

        public async Task<int> SaveChangesAsync<T>() where T : class, IEntity
        {
            return await GetContext<T>().SaveChangesAsync();
        }

        public void Commit()
        {
            _modelContext.SaveChanges();
        }

        public void Rollback()
        {
            // Descarta todos los cambios no guardados realizados en el contexto
            foreach (var entry in _modelContext.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }

        public bool IsDisposed()
        {
            return _disposed;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _modelContext.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
