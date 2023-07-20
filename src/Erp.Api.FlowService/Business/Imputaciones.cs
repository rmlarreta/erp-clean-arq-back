using AutoMapper;
using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Flow;
using Erp.Api.CustomerService.Business;
using Erp.Api.Domain.Entities;
using Erp.Api.Domain.Repositories;
using Erp.Api.Domain.Services;
using Erp.Api.Infraestructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Erp.Api.FlowService.Business
{
    public class Imputaciones : IImputaciones
    {
        private readonly IRepository<CobReciboDetalle> _detalles;
        private readonly IRepository<CobRecibo> _recibos;
        private readonly IService<BusOperacionPago> _pagos;
        private readonly ICustomerBusiness _customerbusiness;
        private readonly IRepository<CobCuentum> _cuentas;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Imputaciones(IUnitOfWork unitOfWork, IService<BusOperacionPago> pagos, ICustomerBusiness customerBusiness, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _detalles = _unitOfWork.GetRepository<CobReciboDetalle>();
            _cuentas = _unitOfWork.GetRepository<CobCuentum>();
            _recibos = _unitOfWork.GetRepository<CobRecibo>();
            _customerbusiness = customerBusiness;
            _pagos = pagos;
            _mapper = mapper;
        }

        public async Task ImputarAlone(Request request)
        {
            await IsImputado((Guid)request.GuidRecibo!);
            Expression<Func<CobRecibo, bool>> expression = c => c.Id == request.GuidRecibo;
            Expression<Func<CobRecibo, object>>[] includeProperties = new Expression<Func<CobRecibo, object>>[]
            {
              o => o.CobReciboDetalles.Where(x=>x.Cancelado==false)
            };
            var recibo = await _recibos.Get(expression, includeProperties);
            var disponible = recibo.CobReciboDetalles.Where(x=>x.Cancelado==false).Sum(x => x.Monto);
            var operaciones = await _customerbusiness.OperacionesImpagas(recibo.ClienteId);
            if (!operaciones.Any()) throw new Exception("No existen operaciones para imputar");
            foreach (var operacion in operaciones.OrderBy(x => x.Numero))
            {
                foreach (var detalle in recibo.CobReciboDetalles.OrderBy(x => x.Monto))
                {
                    if (operacion.SaldosPendientes >= detalle.Monto)
                    {
                        operacion.SaldosPendientes -= detalle.Monto;
                        detalle.Cancelado = true;
                        _detalles.Update(detalle);
                        break;
                    }
                    else
                    {
                        var montoSobrante = detalle.Monto - operacion.SaldosPendientes;
                        detalle.Monto = (decimal)operacion.SaldosPendientes;
                        detalle.Cancelado = true;
                        CobReciboDetallesInsert nuevoDetalle = new()
                        {
                            Cancelado = false,
                            CodAut = detalle.CodAut,
                            Id = Guid.NewGuid(),
                            Monto = (decimal)montoSobrante,
                            Observacion = detalle.Observacion,
                            PosId = detalle.PosId,
                            Tipo = detalle.Tipo,
                            ReciboId = detalle.ReciboId
                        };
                        _detalles.Add(_mapper.Map<CobReciboDetalle>(nuevoDetalle));
                        _detalles.Update(detalle);
                    };
                } 
                BusOperacionPagoDto busOperacionPago = new()
                {
                    Id = Guid.NewGuid(),
                    OperacionId = operacion.Id,
                    ReciboId = recibo.Id
                };
                await _pagos.Add(_mapper.Map<BusOperacionPago>(busOperacionPago));
            }
            _unitOfWork.Commit();
        }

        public async Task SaldarPago(Request request)
        {
            await IsImputado((Guid)request.GuidRecibo!);
            Expression<Func<CobReciboDetalle, bool>> expression = c => c.ReciboId == request.GuidRecibo;
            Expression<Func<CobReciboDetalle, object>>[] includeProperties = new Expression<Func<CobReciboDetalle, object>>[]
            {
              o => o.TipoNavigation
            };
            List<CobReciboDetalle> detalles = await _detalles.GetAll(expression, includeProperties).ToListAsync();

            BusOperacionPagoDto pagoDto = new()
            {
                Id = Guid.NewGuid(),
                OperacionId = (Guid)request.GuidOperacion!,
                ReciboId = (Guid)request.GuidRecibo!,
            };
            Expression<Func<CobCuentum, bool>> expressioncuenta = c => c.Name == "CUENTA CORRIENTE";
            Expression<Func<CobCuentum, object>>[] includePropertiescuenta = Array.Empty<Expression<Func<CobCuentum, object>>>();
            var cuentacc = await _cuentas.Get(expressioncuenta, includePropertiescuenta);
            cuentacc.Saldo -= detalles.Sum(x => x.Monto);
            await _pagos.Add(_mapper.Map<BusOperacionPago>(pagoDto));
        }

        private Task IsImputado(Guid reciboId)
        {
            if (_pagos.Any(x => x.ReciboId == reciboId & !x.Recibo.CobReciboDetalles.Where(x=>x.Cancelado==false).Any()))
            {
                throw new Exception("Ese Recibo ya ha sido imputado");
            }
            return Task.CompletedTask;
        }
    }
}
