using Erp.Api.Application.Dtos.Providers;
using Erp.Api.Domain.Entities;
using System.Linq.Expressions;

namespace Erp.Api.ProviderService.Services
{
    public interface IDocumentoProveedorService
    {
        Task<List<OpDocumentoProveedorDto>> Listado(Expression<Func<OpDocumentoProveedor, bool>> expression);
        Task<OpDocumentoProveedorDto> GetByExpression(Expression<Func<OpDocumentoProveedor, bool>> expression);
        Task AltaDocumento(OpDocumentoProveedorInsert documento);
        Task PagoDocumento(OpPagoProveedor pago);
    }
}
