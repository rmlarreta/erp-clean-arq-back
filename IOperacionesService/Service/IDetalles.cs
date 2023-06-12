using Erp.Api.Application.Dtos.Operaciones.Detalles;

namespace Erp.Api.OperacionesService.Service
{
    public interface IDetalles
    {

        Task InsertDetalles(List<BusOperacionDetalleDto> lista);
    }
}
