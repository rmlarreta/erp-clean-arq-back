using Erp.Api.Application.Dtos.Operaciones.Commons;

namespace Erp.Api.OperacionesService.Interfaces
{
    public interface IEstadoService
    {
        Task<BusEstadoDto> GetByName(string name);
    }
} 