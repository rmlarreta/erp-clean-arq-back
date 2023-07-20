using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.OperacionesService.Service
{
    public interface IOperaciones
    {
        public abstract Task<BusOperacion> GetOperacion(Guid id);
        public abstract Task<List<BusOperacion>> GetAllOperaciones();
        public abstract Task EliminarOperacion(Guid id);
        public abstract Task<BusOperacion> NuevaOperacion(Request? request);
        public abstract Task<FileStreamResult> Imprimir(Guid guid);
        public abstract Task UpdateOperacion(BusOperacion operacion); 
    }
}
