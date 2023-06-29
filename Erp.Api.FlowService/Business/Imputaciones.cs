using AutoMapper;
using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Flow;
using Erp.Api.Application.Dtos.Operaciones;
using Erp.Api.Domain.Entities;
using Erp.Api.Domain.Repositories;
using Erp.Api.Infraestructure.UnitOfWorks;
using System.Linq.Expressions;
using static Erp.Api.Infrastructure.Enums.EstadoDocumentos;

namespace Erp.Api.FlowService.Business
{
    public class Imputaciones : IImputaciones
    {
        private readonly IRepository<BusOperacion> _operaciones;
        private readonly IRepository<CobRecibo> _recibos;
        private readonly IRepository<BusOperacionPago> _pagos;
        private readonly IRepository<BusEstado> _estados;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Imputaciones(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _operaciones = _unitOfWork.GetRepository<BusOperacion>();
            _recibos = _unitOfWork.GetRepository<CobRecibo>();
            _pagos = _unitOfWork.GetRepository<BusOperacionPago>();
            _estados = _unitOfWork.GetRepository<BusEstado>(); 
            _mapper = mapper;
        }

        public async Task ImputarPago(Request request)
        {
            BusOperacionSumaryDto operacion = new();
            CobRecibo recibo = new();
            var totalRecibo = 0.0m;
            if (request.GuidOperacion != null)
            {
                operacion = _mapper.Map<BusOperacionSumaryDto>(await _operaciones.Get((Guid)request.GuidOperacion));
            }
            if (request.GuidRecibo != null)
            {
                await IsImputado((Guid)request.GuidRecibo);
                recibo = await _recibos.Get((Guid)request.GuidRecibo!);
                totalRecibo = recibo.CobReciboDetalles.Sum(x => x.Monto);

            }

            if (operacion.Total == totalRecibo)
            {
                BusOperacionPagoDto pagoDto = new()
                {
                    Id = Guid.NewGuid(),
                    OperacionId = operacion.Id,
                    ReciboId = recibo.Id,
                };
                Expression<Func<BusEstado, bool>> expression = c => c.Name == Estados.PAGADO.Name;
                Expression<Func<BusEstado, object>>[] includeProperties = Array.Empty<Expression<Func<BusEstado, object>>>();
                operacion.EstadoId = _estados.Get(expression, includeProperties).Result.Id;
                foreach (var item in recibo.CobReciboDetalles)
                {
                    item.Cancelado = true;
                }

                _pagos.Add(_mapper.Map<BusOperacionPago>(pagoDto));
                _operaciones.Update(_mapper.Map<BusOperacion>(operacion));
                _recibos.Update(recibo);
                _unitOfWork.Commit();
            }
        }

        private Task IsImputado(Guid reciboId)
        {
            if (_pagos.Any(x => x.ReciboId == reciboId))
            {
                throw new Exception("Ese Recibo ya ha sido imputado");
            }
            return Task.CompletedTask;
        }
    }
}
