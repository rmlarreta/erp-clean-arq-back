using Erp.Api.Application.Dtos.Operaciones;

namespace Erp.Api.OperacionesService.Interfaces
{
    public interface IOperacionesServices
    {
        Task<BusOperacionSumaryDto> NuevoPresupuesto();
        Task<List<BusOperacionSumaryDto>> GetAllPresupuestos();
        Task<BusOperacionSumaryDto> GetOperacion(Guid guid);
    }
}
