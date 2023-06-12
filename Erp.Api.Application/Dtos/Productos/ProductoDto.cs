namespace Erp.Api.Application.Dtos.Productos
{
    public class ProductoDto
    {
        public Guid Id { get; set; }

        public decimal Cantidad { get; set; }

        public string Plu { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public Guid Rubro { get; set; } 

        public Guid Iva { get; set; }
        
        public decimal Neto { get; set; }

        public decimal Internos { get; set; }

        public decimal Tasa { get; set; }

        public bool Servicio { get; set; }
    }
}
