using Erp.Api.OperacionesService.Models;
using IOperacionesService.Factory;
using Microsoft.Extensions.DependencyInjection;

namespace Erp.Api.OperacionesService.Factories
{
    public class DocumentosFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DocumentosFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public OperacionTemplate<T> CreateDocumento<T>(string tipoDocumento) where T : class
        {

            return tipoDocumento switch
            {
                "PRESUPUESTO" => CreatePresupuesto<T>(),
                _ => throw new ArgumentException($"Tipo de documento '{tipoDocumento}' no válido."),
            };
        }
        private OperacionTemplate<T> CreatePresupuesto<T>() where T : class
        {
            var presupuesto = _serviceProvider.GetRequiredService<Presupuesto<T>>();
            return presupuesto;
        }
    }
}