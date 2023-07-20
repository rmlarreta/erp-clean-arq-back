namespace Erp.Api.Application.Dtos.Users
{
    public class UserUpdateDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = null!;

        public string RealName { get; set; } = null!;

        public Guid Role { get; set; }

        public DateTime EndOfLife { get; set; }

        public bool Active { get; set; }
    }
}
