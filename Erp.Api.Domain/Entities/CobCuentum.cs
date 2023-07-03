namespace Erp.Api.Domain.Entities;

public partial class CobCuentum : Entity
{ 
    public string Name { get; set; } = null!;

    public decimal Saldo { get; set; }

    public virtual ICollection<CobCuentaMovimiento> CobCuentaMovimientos { get; set; } = new List<CobCuentaMovimiento>();

    public virtual ICollection<CobTipoPago> CobTipoPagos { get; set; } = new List<CobTipoPago>();
}
