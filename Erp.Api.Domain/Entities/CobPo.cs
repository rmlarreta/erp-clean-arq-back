namespace Erp.Api.Domain.Entities;

public partial class CobPo :Entity
{ 
    public string Name { get; set; } = null!;

    public string? DeviceId { get; set; }

    public string? Token { get; set; }

    public virtual ICollection<CobReciboDetalle> CobReciboDetalles { get; set; } = new List<CobReciboDetalle>();
}
