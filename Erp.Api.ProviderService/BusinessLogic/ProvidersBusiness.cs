using Erp.Api.Application.Dtos.Providers;
using Erp.Api.Domain.Entities;
using Erp.Api.ProviderService.Services;
using System.Linq.Expressions;
using static Erp.Api.Infrastructure.Enums.EstadoDocumentos;

namespace Erp.Api.ProviderService.BusinessLogic
{
    public class ProvidersBusiness : IProvidersBusiness
    {
        private readonly IDocumentoProveedorService _documentos;

        public ProvidersBusiness(IDocumentoProveedorService documentos)
        {
            _documentos = documentos;
        }

        public async Task<List<OpDocumentoProveedorDto>> GetDocumentosPendientes()
        {
            Expression<Func<OpDocumentoProveedor, bool>> expression = c => c.Estado.Name == Estados.ABIERTO.Name;
            return await _documentos.Listado(expression);
        }
    }
}
