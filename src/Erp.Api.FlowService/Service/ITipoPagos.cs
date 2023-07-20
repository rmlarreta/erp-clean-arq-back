using Erp.Api.Application.Dtos.Flow;

namespace Erp.Api.FlowService.Service
{
    public interface ITipoPagos
    {
        Task<List<CobTipoPagoDto>> GetTipoPagos();
    }
}
