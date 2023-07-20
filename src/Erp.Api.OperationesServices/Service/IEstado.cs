using Erp.Api.Application.Dtos.Operaciones.Commons;

namespace Erp.Api.OperacionesService.Service
{
    public interface IEstado
    {
        Task<BusEstadoDto> GetByName(string name); 
    }
}