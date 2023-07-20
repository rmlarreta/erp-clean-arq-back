using Erp.Api.Application.Dtos.Providers;

namespace Erp.Api.ProviderService.BusinessLogic
{
    public interface IProvidersBusiness
    {
        Task<List<OpDocumentoProveedorDto>> GetDocumentosPendientes();

    }
}
