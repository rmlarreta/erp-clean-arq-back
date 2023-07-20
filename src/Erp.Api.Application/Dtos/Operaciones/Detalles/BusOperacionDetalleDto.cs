namespace Erp.Api.Application.Dtos.Operaciones.Detalles
{
    public class BusOperacionDetalleDto
    {
        public Guid? Id { get; set; }

        public Guid OperacionId { get; set; }

        public decimal Cantidad { get; set; }

        public Guid ProductoId { get; set; }

        public string Codigo { get; set; } = null!;

        public string Detalle { get; set; } = null!;

        public string Rubro { get; set; } = null!;

        public decimal Unitario { get; set; }

        public decimal? IvaValue { get; set; }

        public decimal Internos { get; set; }

        public decimal Facturado { get; set; }

    }
}
