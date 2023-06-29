namespace Erp.Api.Application.Dtos.Flow
{
    public class CobReciboDetallesInsert
    {
        public Guid? Id { get; set; }
        public Guid? ReciboId { get; set; }
        public decimal Monto { get; set; }
        public Guid Tipo { get; set; }
        public string? Observacion { get; set; } = null!;
        public Guid? PosId { get; set; }
        public string? CodAut { get; set; }
        public bool? Cancelado { get; set; }
    }
}
