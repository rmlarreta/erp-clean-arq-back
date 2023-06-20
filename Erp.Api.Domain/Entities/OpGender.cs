namespace Erp.Api.Domain.Entities;

public partial class OpGender : Entity
{
    public string Name { get; set; } = null!;

    public virtual ICollection<OpCliente> OpClientes { get; set; } = new List<OpCliente>();
}
