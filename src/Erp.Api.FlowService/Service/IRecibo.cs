using Erp.Api.Application.Dtos.Flow;
using Erp.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.FlowService.Service
{
    public interface IRecibo
    {
        Task<Guid> InsertRecibo(CobReciboInsert recibo);
        Task<CobRecibo> GetRecibo(Guid guid);
        Task<FileStreamResult> Imprimir(Guid guid);

    }
}
