using Erp.Api.Application.Dtos.Flow;
using Erp.Api.Domain.Entities;

namespace Erp.Api.FlowService.Service
{
    public interface IRecibo
    {
        Task<Guid> InsertRecibo(CobReciboInsert recibo);
        Task<CobRecibo> GetRecibo(Guid guid);
    }
}
