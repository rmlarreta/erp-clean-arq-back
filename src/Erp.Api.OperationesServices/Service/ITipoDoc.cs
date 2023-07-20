using Erp.Api.Application.Dtos.Operaciones.Commons;

namespace Erp.Api.OperacionesService.Service
{
    public interface ITipoDoc
    {
        Task<TipoOperacionDto> GetByName(string name);
    }
}
