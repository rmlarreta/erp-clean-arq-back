using Erp.Api.Application.Dtos.Operaciones;

namespace Erp.Api.OperacionesService.BusinessLogic.Interfaces
{
    public interface IOperacionesBusiness
    {
        Task<BusOperacionSumaryDto> NuevaOperacion(string tipoOperacion);
        Task<List<BusOperacionSumaryDto>> GetAllOperaciones(string tipoOperacion);
        Task<BusOperacionSumaryDto> GetOperacion(string tipoOperacion, Guid guid);
        Task DeleteOperacion(Guid guid);
    }
}
