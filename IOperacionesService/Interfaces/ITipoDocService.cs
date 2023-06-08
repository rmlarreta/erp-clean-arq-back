using Erp.Api.Application.Dtos.Operaciones.Commons;

namespace Erp.Api.OperacionesService.Interfaces
{
    public interface ITipoDocService
    {
        Task<TipoOperacionDto> GetByName(string name);
    }
}
