namespace Erp.Api.Domain.Entities;

public partial class SecUser : Entity
{ 
    public string UserName { get; set; } = null!;

    public string RealName { get; set; } = null!;

    public Guid Role { get; set; }

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public DateTime EndOfLife { get; set; }

    public bool Active { get; set; }

    public virtual SecRole RoleNavigation { get; set; } = null!;
}
