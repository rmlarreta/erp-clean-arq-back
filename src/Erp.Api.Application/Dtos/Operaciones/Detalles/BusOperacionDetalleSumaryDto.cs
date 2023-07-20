namespace Erp.Api.Application.Dtos.Operaciones.Detalles
{
    public class BusOperacionDetalleSumaryDto : BusOperacionDetalleDto
    {
        public decimal CantidadDisponible => Cantidad - Facturado;
        public decimal? TotalInternos => Internos * CantidadDisponible;
        public decimal? TotalNeto10 => IvaValue.Equals(10.5m) ? Math.Round((Unitario - Internos) / 1.105m * CantidadDisponible, 2) : 0.0m;
        public decimal? TotalNeto21 => IvaValue.Equals(21.0m) ? Math.Round((Unitario - Internos) / 1.21m * CantidadDisponible, 2) : 0.0m;
        public decimal? TotalExento => IvaValue.Equals(0.0m) ? Math.Round((Unitario - Internos) * CantidadDisponible, 2) : 0.0m;
        public decimal? TotalIva => Math.Round((decimal)((TotalNeto10 + TotalNeto21) * IvaValue / 100)!, 2);
        public decimal? Total => Math.Round((decimal)(TotalInternos + (TotalNeto10 + TotalNeto21) + TotalIva + TotalExento)!, 2);
        public decimal? TotalIva10 => IvaValue.Equals(10.5m) ? TotalIva : 0.0m;
        public decimal? TotalIva21 => IvaValue.Equals(21.0m) ? TotalIva : 0.0m;
        public decimal? TotalNeto => Math.Round((decimal)(TotalNeto10 + TotalNeto21)!, 2);
        public string? Operador { get; set; }
    }
}
