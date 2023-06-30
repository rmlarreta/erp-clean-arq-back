using AutoMapper;
using Erp.Api.Application.CommonService.Interfaces;
using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Flow;
using Erp.Api.Application.Dtos.Operaciones;
using Erp.Api.Application.Dtos.Operaciones.Commons;
using Erp.Api.CustomerService.Service;
using Erp.Api.Domain.Entities;
using Erp.Api.Domain.Repositories;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.OperacionesService.Service;
using Erp.Api.SecurityService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Erp.Api.Infrastructure.Enums.EstadoDocumentos;
using static Erp.Api.Infrastructure.Enums.TipoDocumentos;

namespace Erp.Api.OperacionesService.ConcreteFactories
{
    public class Remito : Operaciones
    {
        private readonly IRepository<BusOperacionPago> _imputaciones;
        private readonly IRepository<StockProduct> _productos;
        private readonly IRepository<CobReciboDetalle> _recibos;
        public Remito(
            IUnitOfWork unitOfWork,
            ISysEmpresaService empresa,
            ISecurityService security,
            ICustomer customer,
            IEstado estado,
            ITipoDoc tipos,
            IMapper mapper) : base(unitOfWork, empresa, security, customer, estado, tipos, mapper)
        {
            _imputaciones = _unitOfWork.GetRepository<BusOperacionPago>();
            _productos = _unitOfWork.GetRepository<StockProduct>();
            _recibos = _unitOfWork.GetRepository<CobReciboDetalle>();
        }

        public override async Task<BusOperacion> NuevaOperacion(Request? request)
        {
            var operacion = await PrepararDocumento(request!); 

            return _mapper.Map<BusOperacion>(operacion);
        }
        protected async Task<BusOperacionInsert> PrepararDocumento(Request request)
        {
            await this.IsImputado((Guid)request!.GuidRecibo!);

            var operacion = await GetOperacion((Guid)request!.GuidOperacion!);

            await ImputarPago(operacion!, (Guid)request!.GuidRecibo!);

            await ActualizaStock(operacion);

            NumeroLetra numeroLetra = await NumeroLetraDocumento();

           // var operacionInsert = _mapper.Map<BusOperacionInsert>(operacion);

            operacion.EstadoId = EstadoDelDocumento().Result.Id;
            operacion.Numero = numeroLetra.Numero;
            operacion.Fecha = DateTime.Now;
            operacion.TipoDocId = TipoDelDocunento().Result.Id;
            operacion.Vence = DateTime.Now;
            operacion.Operador = _security.GetPerfilAuthenticated();
            await Update(operacion);
            return _mapper.Map<BusOperacionInsert>(operacion);
        }

        protected override async Task<BusEstadoDto> EstadoDelDocumento()
        {
            BusEstadoDto estado = await _estado.GetByName(Estados.ENTREGADO.Name);
            return estado;
        }
        protected override async Task<TipoOperacionDto> TipoDelDocunento()
        {
            TipoOperacionDto tipo = await _tipos.GetByName(TipoDocumento.REMITO.Name);
            return tipo;
        }
        protected override async Task<NumeroLetra> NumeroLetraDocumento()
        {
            SystemIndex? index = await _indexs.GetAll().OrderBy(x => x.Id).FirstOrDefaultAsync();
            NumeroLetra numeroLetra = new()
            {
                Letra = TipoDocumento.REMITO.Code,
                Numero = index!.Remito += 1
            };
            _indexs.Update(index);
            return numeroLetra;
        }

        private async Task IsImputado(Guid reciboId)
        {
            if (await _imputaciones.AnyAsync(x => x.ReciboId == reciboId))
            {
                throw new Exception("Ese Recibo ya ha sido imputado");
            }
        }
        private async Task ImputarPago(BusOperacion operacion, Guid recibo)
        {
            Expression<Func<CobReciboDetalle, bool>> expression = c => c.ReciboId == recibo;
            Expression<Func<CobReciboDetalle, object>>[] includeProperties = Array.Empty<Expression<Func<CobReciboDetalle, object>>>();
            IList<CobReciboDetalle> detalles = await _recibos.GetAll(expression, includeProperties).ToListAsync();
            var totalRecibo = 0.0m;
            foreach (var detalle in detalles)
            {
                totalRecibo += detalle.Monto;
                detalle.Cancelado = true;
            }
            var operacionDto = _mapper.Map<BusOperacionSumaryDto>(operacion);
            if (operacionDto.Total == totalRecibo)
            {
                BusOperacionPagoDto pagoDto = new()
                {
                    Id = Guid.NewGuid(),
                    OperacionId = operacion.Id,
                    ReciboId = recibo,
                };

                _imputaciones.Add(_mapper.Map<BusOperacionPago>(pagoDto));
                _recibos.UpdateRange(detalles.ToList());
            }
        }
        private async Task ActualizaStock(BusOperacion operacion)
        {
            List<StockProduct> productos = new();
            foreach (var detalles in operacion.BusOperacionDetalles)
            {
                var producto = await _productos.Get(detalles.ProductoId);
                producto.Cantidad -= detalles.Cantidad;
                productos.Add(producto);


            };
            _productos.UpdateRange(productos);
        }
    }
}
