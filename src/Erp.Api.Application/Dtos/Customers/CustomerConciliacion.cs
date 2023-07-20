using Erp.Api.Application.Dtos.Flow;
using Erp.Api.Application.Dtos.Operaciones;

namespace Erp.Api.Application.Dtos.Customers
{
    public class CustomerConciliacion
    {
        public OpCustomerDto? Customer { get; set; }
        public IList<BusOperacionSumaryDto>? OperacionesImpagas { get; set; } = new List<BusOperacionSumaryDto>();
        public IList<CobReciboInsert>? RecibosNoImputados { get; set; } = new List<CobReciboInsert>();
        public decimal Debe { get; set; }
        public decimal Haber => RecibosNoImputados!.Any() ? RecibosNoImputados!.Sum(x => x.Detalles!.Sum(s => s.Monto)) : 0.0m;
        public decimal Balance => Debe - Haber;
    }
}
