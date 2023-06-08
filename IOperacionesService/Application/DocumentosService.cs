using Erp.Api.OperacionesService.Factories;
using Erp.Api.OperacionesService.Models;
using IOperacionesService.Interfaces;

namespace IOperacionesService.Application
{
    public class DocumentosService<T> : IDocumentosService<T> where T : class
    {
        private readonly DocumentosFactory _documentoFactory;

        public DocumentosService(DocumentosFactory documentoFactory)
        {
            _documentoFactory = documentoFactory;
        }

        public OperacionTemplate<T> GenerarDocumento(string tipoDocumento)
        {
            return _documentoFactory.CreateDocumento<T>(tipoDocumento);
        }
    }
}
