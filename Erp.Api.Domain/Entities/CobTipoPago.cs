﻿namespace Erp.Api.Domain.Entities;

public partial class CobTipoPago : Entity
{ 
    public string Name { get; set; } = null!;

    public Guid? CuentaId { get; set; }

    public virtual ICollection<CobReciboDetalle> CobReciboDetalles { get; set; } = new List<CobReciboDetalle>();

    public virtual CobCuentum? Cuenta { get; set; }

    public virtual ICollection<OpPago> OpPagos { get; set; } = new List<OpPago>();
}
