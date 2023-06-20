using AutoMapper;
using Erp.Api.Application.CommonService.Interfaces;
using Erp.Api.Application.Dtos.Customers;
using Erp.Api.Application.Dtos.Operaciones;
using Erp.Api.Application.Dtos.Operaciones.Commons;
using Erp.Api.CustomerService.Service;
using Erp.Api.Domain.Entities;
using Erp.Api.Domain.Repositories;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using Erp.Api.SecurityService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Erp.Api.Infrastructure.Enums.EstadoDocumentos;
using static Erp.Api.Infrastructure.Enums.TipoDocumentos;

namespace Erp.Api.OperacionesService.Service
{
    public abstract partial class Operaciones : Service<BusOperacion>, IOperaciones
    {
        private readonly IRepository<SystemIndex> _indexs;
        private readonly ISysEmpresaService _empresa;
        private readonly ISecurityService _security;
        private readonly ICustomer _customer;
        private readonly IEstado _estado;
        private readonly ITipoDoc _tipos;
        private readonly IMapper _mapper;

        protected Operaciones(IUnitOfWork unitOfWork, ISysEmpresaService empresa, ISecurityService security, ICustomer customer, IEstado estado, ITipoDoc tipos, IMapper mapper) : base(unitOfWork)
        {
            _indexs = _unitOfWork.GetRepository<SystemIndex>();
            _empresa = empresa;
            _security = security;
            _customer = customer;
            _estado = estado;
            _tipos = tipos;
            _mapper = mapper;
        }

        public virtual async Task Eliminar(Guid id)
        {
            Expression<Func<BusOperacion, bool>> expression = t => t.TipoDoc.Name == TipoDocumento.PRESUPUESTO.Name && t.Id == id;
            Expression<Func<BusOperacion, object>>[] includeProperties = new Expression<Func<BusOperacion, object>>[]
            {
              o => o.BusOperacionDetalles,
              o => o.BusOperacionObservacions
           };
            var operacion = await Get(expression, includeProperties);
            if (operacion == null) throw new Exception("Esta Operación no se puede eliminar");
            if (operacion.BusOperacionDetalles.Any()) throw new Exception("Debe eliminar los detalles primero");
            if (operacion.BusOperacionObservacions.Any()) throw new Exception("Debe eliminar las Observaciones primero");
            await Delete(operacion);
        }

        public virtual async Task<BusOperacion> GetOperacion(Guid id)
        {
            Expression<Func<BusOperacion, object>>[] includeProperties = new Expression<Func<BusOperacion, object>>[]
            {
              o => o.Cliente,
              o => o.Cliente.RespNavigation,
              o => o.BusOperacionDetalles,
              o => o.BusOperacionObservacions,
              o => o.TipoDoc,
              o => o.Estado
           };
            return await Get(id, includeProperties);
        }

        public virtual async Task<List<BusOperacion>> GetAllOperaciones()
        {
            Expression<Func<BusOperacion, bool>> expression = t => t.TipoDoc.Name == TipoDocumento.PRESUPUESTO.Name;
            Expression<Func<BusOperacion, object>>[] includeProperties = new Expression<Func<BusOperacion, object>>[]
           {
              o => o.Cliente,
              o => o.Cliente.RespNavigation,
              o => o.BusOperacionDetalles,
              o => o.BusOperacionObservacions,
              o => o.TipoDoc,
              o => o.Estado
          };
            return await GetAll(expression, includeProperties).ToListAsync();
        }

        public virtual async Task<BusOperacion> NuevaOperacion(BusOperacion? operacion)
        {
            BusOperacionInsert? nDocumento = await PrepararDocumento();
            await Add(_mapper.Map<BusOperacion>(nDocumento));
            return await Get(nDocumento.Id);
        }

        protected virtual async Task<BusOperacionInsert> PrepararDocumento()
        {
            OpCustomerDto cliente = await ClienteDelDocumento();
            NumeroLetra numeroLetra = await NumeroLetraDocumento();
            BusOperacionInsert operacion = new()
            {
                Operador = _security.GetUserAuthenticated(),
                CodAut = "",
                ClienteId = (Guid)cliente.Id!,
                EstadoId = EstadoDelDocumento().Result.Id,
                Numero = numeroLetra.Numero,
                Fecha = DateTime.Now,
                Razon = cliente.Razon,
                Pos = 0,
                TipoDocId = TipoDelDocunento().Result.Id,
                Vence = DateTime.Now,
                Id = Guid.NewGuid()
            };
            return operacion;
        }

        protected virtual async Task<OpCustomerDto> ClienteDelDocumento()
        {
            return await _customer.GetByCui("0");
        }

        protected virtual async Task<BusEstadoDto> EstadoDelDocumento()
        {
            BusEstadoDto estado = await _estado.GetByName(Estados.ABIERTO.Name);
            return estado;
        }

        protected virtual async Task<TipoOperacionDto> TipoDelDocunento()
        {
            TipoOperacionDto tipo = await _tipos.GetByName(TipoDocumento.PRESUPUESTO.Name);
            return tipo;
        }

        protected virtual async Task<NumeroLetra> NumeroLetraDocumento()
        {
            SystemIndex? index = await _indexs.GetAll().OrderBy(x => x.Id).FirstOrDefaultAsync();
            NumeroLetra numeroLetra = new()
            {
                Letra = TipoDocumento.PRESUPUESTO.Code,
                Numero = index!.Presupuesto += 1
            };
            _indexs.Update(index);
            return numeroLetra;
        }

        async Task IOperaciones.Update(BusOperacion operacion)
        {
            operacion.Operador = _security.GetUserAuthenticated();
            await Update(operacion);
        }

        #region Clases Privadas
        protected class NumeroLetra
        {
            public string? Letra { get; set; }
            public int Numero { get; set; }
        }
        #endregion

    }
}

