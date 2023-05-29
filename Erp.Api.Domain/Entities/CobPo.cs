using System;
using System.Collections.Generic;

namespace Erp.Api.Domain.Entities;

public partial class CobPo
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? DeviceId { get; set; }

    public string? Token { get; set; }

    public virtual ICollection<CobReciboDetalle> CobReciboDetalles { get; set; } = new List<CobReciboDetalle>();
}
