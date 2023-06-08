using Erp.Api.Application.Dtos.Operaciones;

namespace Erp.Api.OperacionesService.Interfaces
{
    public interface IOperacionesServices
    {
        Task<BusOperacionSumaryDto> NuevoPresupuesto();
    }
}
