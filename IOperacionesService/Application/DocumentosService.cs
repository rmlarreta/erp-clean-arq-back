using Erp.Api.OperacionesService.Factories;
using Erp.Api.OperacionesService.Models;
using Erp.Api.OperacionesService.Service;
using IOperacionesService.Interfaces;

namespace IOperacionesService.Application
{
    public class DocumentosService<T> : IDocumentosService<T> where T : class
    {
        private readonly DocumentosFactory _documentoFactory;
        private readonly IDetalles _detalles;

        public DocumentosService(DocumentosFactory documentoFactory, IDetalles detalles)
        {
            _documentoFactory = documentoFactory;
            _detalles = detalles;
        }

        public OperacionTemplate<T> GenerarDocumento(string tipoDocumento)
        {
            return _documentoFactory.CreateDocumento<T>(tipoDocumento);
        }

        public async Task InsertDetalles(List<T> detalles, string tipoDocumento)
        {
            await _documentoFactory.InsertDetalles(detalles, tipoDocumento);
        }

        public async Task<List<T>> ListadoDocumentos(string tipoDocumento)
        {
            return await _documentoFactory.GetAllDocumentos<T>(tipoDocumento);
        }
    }
}
