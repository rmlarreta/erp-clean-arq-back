using Erp.Api.Domain.Repositories;
using Erp.Api.Infraestructure.DbContexts;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Repositories;

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
        int IUnitOfWork.SaveChanges<T>()
        {
            return GetContext<T>().SaveChanges();
        }

        async Task<int> IUnitOfWork.SaveChangesAsync<T>()
        {
            return await GetContext<T>().SaveChangesAsync();
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
    }
}
