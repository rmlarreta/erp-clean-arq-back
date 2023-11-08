using Erp.Api.Application.Dtos.Customers;

namespace Erp.Api.Application.Dtos.Providers
{
    public class OpConciliacionProviders
    {
        public OpCustomerDto Proveedor { get; set; } = null!;
        public List<OpDocumentoProveedorDto> Documentos { get; set; } = new List<OpDocumentoProveedorDto>();
        public decimal Total => Documentos.Sum(x => x.Monto);
    }
}

