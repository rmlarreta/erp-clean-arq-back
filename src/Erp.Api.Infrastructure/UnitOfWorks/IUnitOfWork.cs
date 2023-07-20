using Erp.Api.Domain.Repositories;
using Erp.Api.Infraestructure.DbContexts;

namespace Erp.Api.Infraestructure.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : class, IEntity;

        ErpDbContext GetContext<T>() where T : class, IEntity;

        ErpDbContext GetModelContext();

        int SaveChanges<T>() where T : class, IEntity;

        Task<int> SaveChangesAsync<T>() where T : class, IEntity;

        void Commit();

        void Rollback();

        bool IsDisposed();
    }
}
