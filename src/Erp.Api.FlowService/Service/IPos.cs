using Erp.Api.Application.Dtos.Flow;

namespace Erp.Api.FlowService.Service
{
    public interface IPos
    {
        Task<List<PosDto>> GetPos();
    }
}
