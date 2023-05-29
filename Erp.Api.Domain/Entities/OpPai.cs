using System;
using System.Collections.Generic;

namespace Erp.Api.Domain.Entities;

public partial class OpPai
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<OpCliente> OpClientes { get; set; } = new List<OpCliente>();
}
