using AutoMapper;
using Erp.Api.Application.Dtos.Customers;
using Erp.Api.Application.Dtos.Flow;
using Erp.Api.Application.Dtos.Operaciones;
using Erp.Api.CustomerService.Service;
using Erp.Api.Domain.Entities;
using Erp.Api.Infraestructure.UnitOfWorks;
using System.Linq.Expressions;

namespace Erp.Api.CustomerService.Business
{
    public class CustomerBusiness : Customer, ICustomerBusiness
    {
        public CustomerBusiness(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public async Task<CustomerConciliacion> GetConciliacion(Guid CustomerId)
        {
            var customer = await GetById(CustomerId);
            var impagas = await OperacionesImpagas(CustomerId);
            var recibos = await RecibosNoImputados(CustomerId);
            var montoRecibos = recibos.SelectMany(recibo => recibo.Detalles)
                              .Sum(detalle => detalle.Monto);
            var debt = await GetCCorrientes(CustomerId);
            CustomerConciliacion conciliacion = new()
            {
                Customer = customer,
                OperacionesImpagas = impagas,
                RecibosNoImputados = recibos,
                Debe = debt + montoRecibos
            };
            return conciliacion;

        }

        private async Task<IList<BusOperacionSumaryDto>> OperacionesImpagas(Guid CustomerId)
        {
            Expression<Func<BusOperacion, bool>> expression = operacion => operacion.ClienteId == CustomerId &&
            operacion.BusOperacionPagos.Any(pago =>
            pago.Recibo.CobReciboDetalles.Any(detalle =>
            detalle.TipoNavigation.Name == "CUENTA CORRIENTE")
            );
            Expression<Func<BusOperacion, object>>[] includeProperties = new Expression<Func<BusOperacion, object>>[]
             {
              o => o.Cliente,
              o => o.Cliente.RespNavigation,
              o => o.BusOperacionDetalles,
              o => o.BusOperacionObservacions,
              o => o.TipoDoc,
              o => o.Estado
             };
            var operaciones = await Task.FromResult(_operaciones.GetAll(expression, includeProperties));
            return _mapper.Map<List<BusOperacionSumaryDto>>(operaciones);
        }

        private async Task<IList<CobReciboInsert>> RecibosNoImputados(Guid CustomerId)
        {
            Expression<Func<CobRecibo, bool>> expression = recibo => recibo.ClienteId == CustomerId &&
              !recibo.BusOperacionPagos.Any(pago =>
              pago.ReciboId == recibo.Id);
            Expression<Func<CobRecibo, object>>[] includeProperties = new Expression<Func<CobRecibo, object>>[]
           {
              o => o.CobReciboDetalles
           };
            var recibos = await Task.FromResult(_recibos.GetAll(expression, includeProperties));
            return _mapper.Map<List<CobReciboInsert>>(recibos);
        }

        private async Task<decimal> GetCCorrientes(Guid CustomerId)
        {
            Expression<Func<CobRecibo, bool>> expression = recibo =>
            recibo.ClienteId == CustomerId &&
            recibo.CobReciboDetalles.Any(detalle => detalle.TipoNavigation.Name == "CUENTA CORRIENTE");

            Expression<Func<CobRecibo, object>>[] includeProperties = new Expression<Func<CobRecibo, object>>[]
            {
            o => o.CobReciboDetalles
            };

            var recibos = await Task.FromResult(_recibos.GetAll(expression, includeProperties));
            return recibos.SelectMany(recibo => recibo.CobReciboDetalles)
                                          .Where(detalle => detalle.TipoNavigation.Name == "CUENTA CORRIENTE")
                                          .Sum(detalle => detalle.Monto);
        }
    }
}
