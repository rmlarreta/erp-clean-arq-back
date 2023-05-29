using System;
using System.Collections.Generic;

namespace Erp.Api.Domain.Entities;

public partial class BusOperacionTipo
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Code { get; set; }

    public string? CodeExt { get; set; }

    public int? TipoAfip { get; set; }

    public virtual ICollection<BusOperacion> BusOperacions { get; set; } = new List<BusOperacion>();

    public virtual ICollection<OpDocumentoProveedor> OpDocumentoProveedors { get; set; } = new List<OpDocumentoProveedor>();
}
