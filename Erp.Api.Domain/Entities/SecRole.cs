using System;
using System.Collections.Generic;

namespace Erp.Api.Domain.Entities;

public partial class SecRole
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<SecUser> SecUsers { get; set; } = new List<SecUser>();
}
