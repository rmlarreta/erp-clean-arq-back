namespace Erp.Api.Domain.Entities;

public partial class StockRubro : Entity
{

    public string Name { get; set; } = null!;

    public virtual ICollection<StockProduct> StockProducts { get; set; } = new List<StockProduct>();
}
