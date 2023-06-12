using AutoMapper;
using Erp.Api.CustomerService.Service;
using Erp.Api.Domain.Entities;
using Erp.Api.Domain.Repositories;
using Erp.Api.Domain.Services;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.OperacionesService.Service;
using Erp.Api.SecurityService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Aramis.Api.Repository.Enumss.TipoDocumentos;

namespace Erp.Api.OperacionesService.Models
{
    public abstract partial class OperacionTemplate<T> where T : class
    {
        private readonly IRepository<SystemIndex> _indexs;
        private readonly IRepository<BusOperacion> _operaciones;
        private readonly IService<BusOperacionDetalle> _detalles;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISecurityService _security;
        private readonly ICustomer _customer;
        private readonly IEstado _estado;
        private readonly ITipoDoc _tipos;
        private readonly IMapper _mapper;

        protected OperacionTemplate(IUnitOfWork unitOfWork, IService<BusOperacionDetalle> detalles, ISecurityService security, ICustomer customer, IEstado estado, ITipoDoc tipos, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _indexs = _unitOfWork.GetRepository<SystemIndex>();
            _operaciones = _unitOfWork.GetRepository<BusOperacion>();
            _detalles = detalles;
            _security = security;
            _customer = customer;
            _estado = estado;
            _tipos = tipos;
            _mapper = mapper;
        }
        public virtual async Task<List<BusOperacion>> GetAll()
        {
            Expression<Func<BusOperacion, bool>> expression = c => c.TipoDoc.Name == TipoDocumento.PRESUPUESTO.Name;
            Expression<Func<BusOperacion, object>>[] includeProperties = new Expression<Func<BusOperacion, object>>[]
            {
              o => o.Cliente,
              o => o.Cliente.RespNavigation,
              o => o.BusOperacionDetalles,
              o => o.BusOperacionObservacions,
              o => o.TipoDoc,
              o => o.Estado
            };

            return await _operaciones.GetAll(expression, includeProperties).ToListAsync();
        }
        public virtual async Task<BusOperacion> GetById(Guid id)
        {
            Expression<Func<BusOperacion, bool>> expression = c => c.TipoDoc.Name == TipoDocumento.PRESUPUESTO.Name;
            Expression<Func<BusOperacion, object>>[] includeProperties = new Expression<Func<BusOperacion, object>>[]
            {
              o => o.Cliente,
              o => o.Cliente.RespNavigation,
              o => o.BusOperacionDetalles,
              o => o.BusOperacionObservacions,
              o => o.TipoDoc,
              o => o.Estado
            };

            return await _operaciones.Get(id, includeProperties);
        }
        public virtual async Task InsertDetalles(List<BusOperacionDetalle> detalles)
        {
            await _detalles.AddRange(detalles);
        }
        public virtual async Task UpdateDetalles(List<BusOperacionDetalle> detalles)
        {
            await _detalles.UpdateRange(detalles);
        }
        public abstract Task Imprimir();
    }
}
