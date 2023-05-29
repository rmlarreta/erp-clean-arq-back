using System;
using System.Collections.Generic;

namespace Erp.Api.Domain.Entities;

public partial class CobRecibo
{
    public Guid Id { get; set; }

    public Guid ClienteId { get; set; }

    public DateTime Fecha { get; set; }

    public string Operador { get; set; } = null!;

    public int Numero { get; set; }

    public virtual ICollection<BusOperacionPago> BusOperacionPagos { get; set; } = new List<BusOperacionPago>();

    public virtual OpCliente Cliente { get; set; } = null!;

    public virtual ICollection<CobReciboDetalle> CobReciboDetalles { get; set; } = new List<CobReciboDetalle>();
}
