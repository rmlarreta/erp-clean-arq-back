using Erp.Api.Application.Dtos.Customers;
using Erp.Api.Application.Dtos.Operaciones.Commons;

namespace Erp.Api.Application.Dtos.Providers
{
    public class OpDocumentoProveedorDto
    {
        public Guid Id { get; set; } 

        public DateTime Fecha { get; set; }

        public string Razon { get; set; } = null!;  

        public int Pos { get; set; }

        public int Numero { get; set; }

        public decimal Monto { get; set; }

        public BusEstadoDto Estado { get; set; } = null!;
          
        public OpCustomerDto Proveedor { get; set; } = null!;

        public TipoOperacionDto TipoDoc { get; set; } = null!;
    }
}
