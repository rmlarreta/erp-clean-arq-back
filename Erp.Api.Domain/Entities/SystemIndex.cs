namespace Erp.Api.Domain.Entities;

public partial class SystemIndex : Entity
{
    public int Remito { get; set; }

    public int Presupuesto { get; set; }

    public int Recibo { get; set; }

    public int Orden { get; set; }

    public bool Production { get; set; }

    public decimal Interes { get; set; }
}
