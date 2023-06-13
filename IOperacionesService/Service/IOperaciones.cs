using Erp.Api.Domain.Entities;

namespace Erp.Api.OperacionesService.Service
{
    public interface IOperaciones 
    {
        public abstract Task<BusOperacion> GetOperacion(Guid id);
        public abstract Task<List<BusOperacion>> GetAllOperaciones();
        public abstract Task Eliminar(Guid id);
        public abstract Task<BusOperacion> NuevaOperacion(BusOperacion? operacion);
        public abstract Task Imprimir();
    }
}
