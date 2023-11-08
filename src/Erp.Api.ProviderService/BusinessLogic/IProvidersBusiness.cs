using Erp.Api.Application.Dtos.Providers;

namespace Erp.Api.ProviderService.BusinessLogic
{
    public interface IProvidersBusiness
    {
        Task<List<OpConciliacionProviders>> GetDocumentosPendientes();
        Task AltaDocumento(OpDocumentoProveedorInsert documento);
        Task Pago(OpPagoProveedor pago);

    }
}
