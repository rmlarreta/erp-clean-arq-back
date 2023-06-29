namespace Erp.Api.Application.Dtos.Flow
{
    public class CobTipoPagoDto
    {
        public Guid? Id { get; set; }

        public string Name { get; set; } = null!;

        public Guid? CuentaId { get; set; }
    }
}
