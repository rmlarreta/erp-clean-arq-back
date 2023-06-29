using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Operaciones;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.OperacionesService.BusinessLogic.Interfaces
{
    public interface IOperacionesBusiness
    {
        Task<BusOperacionSumaryDto> NuevaOperacion(string tipoOperacion, Request? request);
        Task<List<BusOperacionSumaryDto>> GetAllOperaciones(string tipoOperacion);
        Task<BusOperacionSumaryDto> GetOperacion(string tipoOperacion, Guid guid);
        Task DeleteOperacion(Guid guid);
        Task UpdateOperacion(BusOperacionInsert operacion);
        Task<FileStreamResult> Imprimir(Guid guid);
    }
}
