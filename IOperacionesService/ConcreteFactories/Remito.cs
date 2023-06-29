using AutoMapper;
using Erp.Api.Application.CommonService.Interfaces;
using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Operaciones.Commons;
using Erp.Api.Application.Dtos.Productos;
using Erp.Api.CustomerService.Service;
using Erp.Api.Domain.Entities;
using Erp.Api.FlowService.Business;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.OperacionesService.Service;
using Erp.Api.ProductosService.Service;
using Erp.Api.SecurityService.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Erp.Api.Infrastructure.Enums.EstadoDocumentos;
using static Erp.Api.Infrastructure.Enums.TipoDocumentos;

namespace Erp.Api.OperacionesService.ConcreteFactories
{
    public class Remito : Operaciones
    {
        private readonly IImputaciones _imputaciones;
        private readonly IProductos _productos;

        public Remito(
            IUnitOfWork unitOfWork,
            ISysEmpresaService empresa,
            ISecurityService security,
            ICustomer customer,
            IEstado estado,
            ITipoDoc tipos,
            IMapper mapper,
            IImputaciones imputaciones,
            IProductos productos) : base(unitOfWork, empresa, security, customer, estado, tipos, mapper)
        {
            _imputaciones = imputaciones;
            _productos = productos;
        }

        public override async Task<BusOperacion> NuevaOperacion(Request? request)
        {

            await _imputaciones.ImputarPago(request!);
            var operacion = await GetOperacion((Guid)request!.GuidOperacion!);
            await ActualizaStock(operacion);
            operacion.Estado = _mapper.Map<BusEstado>(await EstadoDelDocumento());
            operacion.TipoDoc = _mapper.Map<BusOperacionTipo>(await TipoDelDocunento());
            operacion.Fecha = DateTime.Now;
            NumeroLetra numeroLetra = await NumeroLetraDocumento();
            operacion.Numero = numeroLetra.Numero;
            operacion.Vence = DateTime.Now;
            await UpdateOperacion(operacion);

            return operacion;
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
        private async Task ActualizaStock(BusOperacion operacion)
        {
            List<ProductoDto> productos = new();
            foreach (var detalles in operacion.BusOperacionDetalles)
            {
                var producto = await _productos.GetById(detalles.ProductoId);
                producto.Cantidad -= detalles.Cantidad;
                productos.Add(producto);
            };
            await _productos.UpdateRangeProducto(productos);
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

    }
}
