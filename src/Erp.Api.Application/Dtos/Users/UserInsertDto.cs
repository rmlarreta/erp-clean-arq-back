namespace Erp.Api.Application.Dtos.Users
{
    public class UserInsertDto
    {
        public string UserName { get; set; } = null!;
        public string RealName { get; set; } = null!;
        public string PassWord { get; set; } = null!;
        public Guid Role { get; set; }

    }
}
