﻿using Erp.Api.Domain.Repositories;
using Erp.Api.Domain.Services;
using Erp.Api.Infraestructure.UnitOfWorks;
using System.Linq.Expressions;

namespace Erp.Api.Infrastructure.Data.Services
{
    public class Service<TEntity> : IService<TEntity> where TEntity : class, IEntity
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly IUnitOfWork _unitOfWork;

        public Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<TEntity>();
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                return _repository.Any(expression);
            }
            catch (Exception)
            {

                throw new Exception("Error al obtener las Entidades");
            }
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                return await _repository.AnyAsync(expression);
            }
            catch (Exception)
            {

                throw new Exception("Error al obtener las Entidades");
            }
        }

        public virtual async Task<int> Add(TEntity data)
        {
            _repository.Add(data);
            return await _unitOfWork.SaveChangesAsync<TEntity>();
        }

        public virtual async Task<int> AddRange(List<TEntity> data)
        {
            _repository.AddRange(data);
            return await _unitOfWork.SaveChangesAsync<TEntity>();
        }

        public virtual async Task<int> Delete(Guid id)
        {
            return await Delete(await Get(id));
        }

        public virtual async Task<int> DeleteRange(List<TEntity> data)
        {
            _repository.DeleteRange(data);
            return await _unitOfWork.SaveChangesAsync<TEntity>();
        }

        public virtual async Task<int> Delete(TEntity data)
        {
            _repository.Delete(data);
            return await _unitOfWork.SaveChangesAsync<TEntity>();
        }

        public async Task<bool> Exist(Guid id)
        {
            return await _repository.AnyAsync(e => e.Id.Equals(id));
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.GetAll(includeProperties);
        }

        public virtual async Task<TEntity> Get(Guid id)
        {
            return await _repository.Get(id);
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.GetAll(expression, includeProperties);
        }

        public virtual async Task<TEntity> Get(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await _repository.Get(expression, includeProperties);
        }

        public virtual async Task<TEntity> Get(Guid id, Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await _repository.Get(id, includeProperties);
        }

        public async Task<int> Update(TEntity data)
        {
            _repository.Update(data);
            return await _unitOfWork.SaveChangesAsync<TEntity>();
        }

        public async Task<int> UpdateRange(List<TEntity> data)
        {
            _repository.UpdateRange(data);
            return await _unitOfWork.SaveChangesAsync<TEntity>();
        }

        public virtual async Task<TEntity> GetReloadAsync(Guid id)
        {
            return await _repository.GetReloadAsync(id);
        }

    }
}
