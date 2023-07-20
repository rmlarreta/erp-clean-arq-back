namespace Erp.Api.Application.Dtos.Productos
{
    public class ProductoSummaryDto : ProductoDto
    {
        public string? RubroName { get; set; }

        public decimal? IvaValue { get; set; }

        public decimal? Unitario => (Neto * (1 + (IvaValue / 100)) * (1 + (Tasa / 100))) + Internos;

    }
}
