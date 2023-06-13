using Erp.Api.Application.Dtos.Productos;

namespace Erp.Api.ProductosService.Service
{
    public interface IProductos
    {
        Task<List<ProductoSummaryDto>> Listado();
        Task InsertProducto(ProductoDto producto);
        Task DeleteProducto(Guid guid);
        Task UpdateProducto(ProductoDto producto);
    }
}
