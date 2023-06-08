using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Operaciones.Detalles;
using Erp.Api.Application.Dtos.Operaciones.Observaciones;
using Erp.Api.Infrastructure.Helpers;

namespace Erp.Api.Application.Dtos.Operaciones
{
    public class BusOperacionSumaryDto : BusOperacionDto
    {
        public decimal Total => Detalles!.Sum(x => x.Total) ?? 0.0m;

        public string? TotalLetras => ExtensionMethods.NumeroLetras(Total);

        public decimal? TotalInternos => Detalles!.Sum(x => x.TotalInternos);

        public decimal? TotalNeto => Detalles!.Sum(x => x.TotalNeto);

        public decimal? TotalIva => Detalles!.Sum(x => x.TotalIva);

        public decimal? TotalIva10 => Detalles!.Sum(x => x.TotalIva10);

        public decimal? TotalIva21 => Detalles!.Sum(x => x.TotalIva21);

        public decimal? TotalExento => Detalles!.Sum(x => x.TotalExento);

        public List<BusOperacionDetalleSumaryDto> Detalles { get; set; } = new();

        public List<BusOperacionesObservacionDto> Observaciones { get; set; } = new();

        public SysEmpresaDto Empresa { get; set; } = new();
    }
}
