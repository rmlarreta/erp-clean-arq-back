using Erp.Api.OperacionesService.Models;

namespace IOperacionesService.Interfaces
{
    public interface IDocumentosService<T> where T : class
    {
        OperacionTemplate<T> GenerarDocumento(string tipoDocumento);
        Task<List<T>> ListadoDocumentos(string tipoDocumento);
    }

}

