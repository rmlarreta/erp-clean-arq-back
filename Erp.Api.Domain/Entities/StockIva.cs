namespace Erp.Api.Domain.Entities;

public partial class StockIva : Entity
{
    public decimal Value { get; set; }

    public virtual ICollection<StockProduct> StockProducts { get; set; } = new List<StockProduct>();
}
