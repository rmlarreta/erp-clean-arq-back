using System;
using System.Collections.Generic;

namespace Erp.Api.Domain.Entities;

public partial class SystemIndex
{
    public Guid Id { get; set; }

    public int Remito { get; set; }

    public int Presupuesto { get; set; }

    public int Recibo { get; set; }

    public int Orden { get; set; }

    public bool Production { get; set; }

    public decimal Interes { get; set; }
}
