namespace Erp.Api.Domain.Entities;

public partial class OpResp : Entity
{
    public string Name { get; set; } = null!;

    public virtual ICollection<OpCliente> OpClientes { get; set; } = new List<OpCliente>();
}
