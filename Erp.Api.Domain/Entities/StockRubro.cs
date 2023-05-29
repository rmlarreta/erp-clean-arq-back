using System;
using System.Collections.Generic;

namespace Erp.Api.Domain.Entities;

public partial class StockRubro
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<StockProduct> StockProducts { get; set; } = new List<StockProduct>();
}
