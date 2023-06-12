﻿namespace Erp.Api.Domain.Entities;

public partial class OpGender
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<OpCliente> OpClientes { get; set; } = new List<OpCliente>();
}
