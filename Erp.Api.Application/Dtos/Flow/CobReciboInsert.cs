namespace Erp.Api.Application.Dtos.Flow
{
    public class CobReciboInsert
    {
        public Guid? Id { get; set; }
        public int? Numero { get; set; }
        public Guid ClienteId { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Operador { get; set; } = null!;
        public string? Operacion { get; set; } = null!; 
        public List<CobReciboDetallesInsert>? Detalles { get; set; } = new List<CobReciboDetallesInsert>();
    }
}
