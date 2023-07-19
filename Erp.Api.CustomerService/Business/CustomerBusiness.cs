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
            var debt = impagas.Sum(x => x.SaldosPendientes);
            CustomerConciliacion conciliacion = new()
            {
                Customer = customer,
                OperacionesImpagas = impagas,
                RecibosNoImputados = recibos,
                Debe = (decimal)debt!
            };
            return conciliacion;

        }

        public async Task<IList<BusOperacionSumaryDto>> OperacionesImpagas(Guid CustomerId)
        {
            Expression<Func<BusOperacion, bool>> expression = operacion =>
                operacion.ClienteId == CustomerId && operacion.BusOperacionPagos.Any();

            Expression<Func<BusOperacion, object>>[] includeProperties = new Expression<Func<BusOperacion, object>>[]
            {
        o => o.Cliente,
        o => o.Cliente.RespNavigation,
        o => o.BusOperacionDetalles,
        o => o.BusOperacionObservacions,
        o => o.TipoDoc,
        o => o.Estado,
        o => o.BusOperacionPagos
            };

            var operaciones = await Task.FromResult(_operaciones.GetAll(expression, includeProperties));

            var operacionesDto = _mapper.Map<List<BusOperacionSumaryDto>>(operaciones);
            foreach (var operacionDto in operacionesDto)
            {
                operacionDto.SaldosPendientes = operacionDto.Total - await SaldosRecibo(operacionDto.Pagos!);
            }
            operacionesDto.RemoveAll(x => x.SaldosPendientes <= 0);
            return operacionesDto;
        }

        private async Task<decimal> SaldosRecibo(List<BusOperacionPagoDto> Pagos)
        {
            var total = 0.0m;
            foreach (var pago in Pagos)
            {
                Expression<Func<CobRecibo, bool>> expression = recibo => recibo.Id == pago.ReciboId;
                Expression<Func<CobRecibo, object>>[] includeProperties = new Expression<Func<CobRecibo, object>>[]
               {
              o => o.CobReciboDetalles.Where(x=>x.Cancelado ==true)
               };
                var result = await _recibos.Get(expression, includeProperties);
                total += result.CobReciboDetalles.Sum(x => x.Monto);
            }
            return total;
        }
        private async Task<IList<CobReciboInsert>> RecibosNoImputados(Guid CustomerId)
        {
            Expression<Func<CobRecibo, bool>> filter = recibo => recibo.ClienteId == CustomerId && recibo.CobReciboDetalles.Any(x => x.Cancelado==false);
            Expression<Func<CobRecibo, object>>[] includeProperties = new Expression<Func<CobRecibo, object>>[]
            {
        o => o.CobReciboDetalles.Where(x =>  x.Cancelado==false)
            };

            var recibos =await Task.FromResult(_recibos.GetAll(filter, includeProperties));
            foreach (var recibo in recibos)
            {
                recibo.CobReciboDetalles = recibo.CobReciboDetalles.Where(x => x.Cancelado == false).ToList();
            }


            return _mapper.Map<List<CobReciboInsert>>(recibos);
        }
    }
}
