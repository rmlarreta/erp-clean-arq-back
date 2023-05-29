using System;
using System.Collections.Generic;

namespace Erp.Api.Domain.Entities;

public partial class CobCuentaMovimiento
{
    public Guid Id { get; set; }

    public Guid Cuenta { get; set; }

    public bool Debito { get; set; }

    public bool Computa { get; set; }

    public string Detalle { get; set; } = null!;

    public decimal Monto { get; set; }

    public DateTime Fecha { get; set; }

    public string Operador { get; set; } = null!;

    public virtual CobCuentum CuentaNavigation { get; set; } = null!;
}
