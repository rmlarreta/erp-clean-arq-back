using AutoMapper;
using Erp.Api.Domain.Entities;
using Erp.Api.OperacionesService.Models;
using IOperacionesService.Factory;
using Microsoft.Extensions.DependencyInjection;

namespace Erp.Api.OperacionesService.Factories
{
    public class DocumentosFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        public DocumentosFactory(IServiceProvider serviceProvider, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }

        public OperacionTemplate<T> CreateDocumento<T>(string tipoDocumento) where T : class
        {

            return tipoDocumento switch
            {
                "PRESUPUESTO" => CreatePresupuesto<T>(),
                _ => throw new ArgumentException($"Tipo de documento '{tipoDocumento}' no válido."),
            };
        }

        public async Task<List<T>> GetAllDocumentos<T>(string tipoDocumento) where T : class
        {
            return tipoDocumento switch
            {
                "PRESUPUESTO" => await GetPresupuestos<T>(),
                _ => throw new ArgumentException($"Tipo de documento '{tipoDocumento}' no válido."),
            };
        }

        public async Task InsertDetalles<T>(List<T> data, string tipoDocumento) where T : class
        {
            switch (tipoDocumento)
            {
                case "PRESUPUESTO":
                    await InsertDetallesPresupuesto<T>(data);
                    break;
                default:
                    throw new ArgumentException($"Tipo de documento '{tipoDocumento}' no válido.");
            }
        }

        #region PRIVATE PRESUPUESTOS
        private async Task<List<T>> GetPresupuestos<T>() where T : class
        {
            OperacionTemplate<BusOperacion>? presupuesto = _serviceProvider.GetRequiredService<OperacionTemplate<BusOperacion>>();
            List<BusOperacion>? presupuestos = await presupuesto.GetAll();

            List<T>? mappedPresupuestos = _mapper.Map<List<T>>(presupuestos);
            return mappedPresupuestos;
        }
        private OperacionTemplate<T> CreatePresupuesto<T>() where T : class
        {
            Presupuesto<T>? presupuesto = _serviceProvider.GetRequiredService<Presupuesto<T>>();
            return presupuesto;
        }

        private async Task InsertDetallesPresupuesto<T>(List<T> data) where T : class
        {
            Presupuesto<T>? presupuesto = _serviceProvider.GetRequiredService<Presupuesto<T>>();
            List<BusOperacionDetalle>? detalles = _mapper.Map<List<BusOperacionDetalle>>(data);
            await presupuesto.InsertDetalles(detalles);
        }

        #endregion
    }
}