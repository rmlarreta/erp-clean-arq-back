namespace Erp.Api.Application.Dtos.Security
{
    public class UserAuth
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string RealName { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
