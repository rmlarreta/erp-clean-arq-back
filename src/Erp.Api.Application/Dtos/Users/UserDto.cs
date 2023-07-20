namespace Erp.Api.Application.Dtos.Users
{
    public class UserDto : UserUpdateDto
    {
        public string RoleName { get; set; } = null!;
    }
}
