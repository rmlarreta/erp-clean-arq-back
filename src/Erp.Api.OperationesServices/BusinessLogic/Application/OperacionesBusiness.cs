using AutoMapper;
using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Operaciones;
using Erp.Api.Domain.Entities;
using Erp.Api.OperacionesService.BusinessLogic.Interfaces;
using Erp.Api.OperacionesService.ConcreteFactories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using static Erp.Api.Infrastructure.Enums.TipoDocumentos;

namespace Erp.Api.OperacionesService.BusinessLogic.Application
{
    public class OperacionesBusiness : IOperacionesBusiness
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        public OperacionesBusiness(IServiceProvider serviceProvider, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }

        public async Task DeleteOperacion(Guid guid)
        {
            await GetPresupuesto().EliminarOperacion(guid);
        }

        public async Task<List<BusOperacionSumaryDto>> GetAllOperaciones(string tipoOperacion)
        {
            return tipoOperacion switch
            {
                "PRESUPUESTO" => _mapper.Map<List<BusOperacionSumaryDto>>(await GetPresupuesto().GetAllOperaciones()),
                _ => throw new ArgumentException("Tipo de operación no válido"),
            };
        }

        public async Task<BusOperacionSumaryDto> GetOperacion(string tipoOperacion, Guid guid)
        {
            return tipoOperacion switch
            {
                "PRESUPUESTO" => _mapper.Map<BusOperacionSumaryDto>(await GetPresupuesto().GetOperacion(guid)),
                _ => throw new ArgumentException("Tipo de operación no válido"),
            };
        }

        public async Task<FileStreamResult> Imprimir(Guid guid)
        {
            var operacion = await _serviceProvider.GetRequiredService<ConcreteOperacion>().GetOperacion(guid);

            return operacion.TipoDoc.Name switch
            {
                "PRESUPUESTO" => await GetPresupuesto().Imprimir(guid),
                "REMITO" => await GetRemito().Imprimir(guid),
                _ => throw new ArgumentException("Tipo de operación no válido"),
            };
        }

        public async Task<BusOperacionSumaryDto> NuevaOperacion(string tipoOperacion, Request? request)
        {
            return tipoOperacion switch
            {
                "PRESUPUESTO" => _mapper.Map<BusOperacionSumaryDto>(await GetPresupuesto().NuevaOperacion(null)),
                "REMITO" => _mapper.Map<BusOperacionSumaryDto>(await GetRemito().NuevaOperacion(request)),
                _ => throw new ArgumentException("Tipo de operación no válido"),
            };
        }

        public async Task UpdateOperacion(BusOperacionInsert operacion)
        {

            ConcreteOperacion operacionBase = _serviceProvider.GetRequiredService<ConcreteOperacion>();
            if (await operacionBase.AnyAsync(
            x => x.TipoDoc.Name == TipoDocumento.PRESUPUESTO.Name &&
            x.TipoDocId == operacion.TipoDocId &&
            x.EstadoId == operacion.EstadoId)) // Es un presupuesto que no cambia de estado ni tipo
            {
                await GetPresupuesto().Update(_mapper.Map<BusOperacion>(operacion));
            }

            //si no presupuesto derivar para obtener base
            //si presupuesto modificar libremente mientras no cambie el estado
        }

        #region Presupuestos
        private Presupuesto GetPresupuesto()
        {
            Presupuesto presupuesto = _serviceProvider.GetRequiredService<Presupuesto>();
            return presupuesto;
        }
        #endregion

        #region Remitos
        private Remito GetRemito()
        {
            Remito remito = _serviceProvider.GetRequiredService<Remito>();
            return remito;
        }
        #endregion 
    }
}
