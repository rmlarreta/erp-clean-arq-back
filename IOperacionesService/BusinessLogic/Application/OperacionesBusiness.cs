using AutoMapper;
using Erp.Api.Application.Dtos.Operaciones;
using Erp.Api.OperacionesService.BusinessLogic.Interfaces;
using Erp.Api.OperacionesService.ConcreteFactories;
using Microsoft.Extensions.DependencyInjection;

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
            await GetPresupuesto().Eliminar(guid);
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

        public async Task<BusOperacionSumaryDto> NuevaOperacion(string tipoOperacion)
        {
            return tipoOperacion switch
            {
                "PRESUPUESTO" => _mapper.Map<BusOperacionSumaryDto>(await GetPresupuesto().NuevaOperacion(null)),
                _ => throw new ArgumentException("Tipo de operación no válido"),
            };
        }

        #region Presupuestos
        private Presupuesto GetPresupuesto()
        {
            var presupuesto = _serviceProvider.GetRequiredService<Presupuesto>();
            return presupuesto;
        }
        #endregion
    }
}
