using Erp.Api.Application.Dtos.Productos;

namespace Erp.Api.ProductosService.Service
{
    public interface IRubros
    {
        Task<List<RubroDto>> GetAllRubros();
        Task DeleteRubro(Guid guid);
        Task UpdateRubro(RubroDto rubro);
        Task InsertRubro(RubroDto rubro);
    }
}
