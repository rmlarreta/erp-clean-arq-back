using Erp.Api.Application.Dtos.Productos;

namespace Erp.Api.ProductosService.Service
{
    public interface IProductos
    {
        Task<ProductoDto> GetById(Guid id);
        Task<List<ProductoSummaryDto>> Listado();
        Task InsertProducto(ProductoDto producto);
        Task DeleteProducto(Guid guid);
        Task UpdateProducto(ProductoDto producto);
        Task UpdateRangeProducto(List<ProductoDto> productos);
    }
}
