using Erp.Api.Application.Dtos.Productos;

namespace Erp.Api.ProductosService.Service
{
    public interface IProductosIva
    {
        Task<List<IvaDto>> GetIvas();
    }
}
