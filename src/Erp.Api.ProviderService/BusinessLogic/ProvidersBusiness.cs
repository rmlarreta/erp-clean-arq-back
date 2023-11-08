using AutoMapper;
using Erp.Api.Application.Dtos.Providers;
using Erp.Api.Domain.Entities;
using Erp.Api.ProviderService.Services;
using System.Linq.Expressions;
using static Erp.Api.Infrastructure.Enums.EstadoDocumentos;
using static Erp.Api.Infrastructure.Enums.TipoDocumentos;

namespace Erp.Api.ProviderService.BusinessLogic
{
    public class ProvidersBusiness : IProvidersBusiness
    {
        private readonly IDocumentoProveedorService _documentos;
        private readonly IMapper _mapper;

        public ProvidersBusiness(IDocumentoProveedorService documentos, IMapper mapper)
        {
            _documentos = documentos;
            _mapper = mapper;
        }

        public async Task AltaDocumento(OpDocumentoProveedorInsert documento)
        {
            await _documentos.AltaDocumento(documento);
        }

        public async Task<List<OpConciliacionProviders>> GetDocumentosPendientes()
        {
            Expression<Func<OpDocumentoProveedor, bool>> expression = c => c.Estado.Name == Estados.ABIERTO.Name && !(c.TipoDoc.Name == TipoDocumento.DEVOLUCION.Name || c.TipoDoc.Name.StartsWith("NOTA DE CREDITO"));
            var documentos = await _documentos.Listado(expression);
            var documentosAgrupados = documentos.GroupBy(x => x.Proveedor.Id);
            List<OpConciliacionProviders> listadoConciliaciones = new();
            foreach (var prov in documentosAgrupados)
            {
                OpConciliacionProviders opConciliacionProviders = new()
                {
                    Proveedor = prov.FirstOrDefault()!.Proveedor,
                    Documentos = prov.ToList()
                };
                listadoConciliaciones.Add(opConciliacionProviders);
            };
            return listadoConciliaciones;
        }

        public async Task Pago(OpPagoProveedor pago)
        {
            await _documentos.PagoDocumento(pago);
        }
    }
}
