namespace Erp.Api.Domain.Entities;

public partial class SecRole : Entity
{
    public string Name { get; set; } = null!;

    public virtual ICollection<SecUser> SecUsers { get; set; } = new List<SecUser>();
}
