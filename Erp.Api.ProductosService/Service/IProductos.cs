using Erp.Api.Application.Dtos.Productos;

namespace Erp.Api.ProductosService.Service
{
    public interface IProductos
    {
        Task<List<ProductoSummaryDto>> Listado();
    }
}
