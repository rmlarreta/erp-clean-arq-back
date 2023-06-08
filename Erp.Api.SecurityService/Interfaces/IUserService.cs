using Erp.Api.Application.Dtos.Users;
using Erp.Api.Application.Dtos.Users.Commons;
using Erp.Api.Domain.Entities;

namespace Erp.Api.SecurityService.Interfaces
{
    public interface IUserService
    {
        Task<SecUser> GetUserBaseByName(string username);
        Task CreateUser(UserInsertDto user);
        Task UpdateUser(UserUpdateDto user);
        Task DeleteUser(Guid id);
        IEnumerable<UserDto> GetAllUsers();
        Task<UserDto> GetUserById(Guid id);
        Task<UserDto> GetUserByName(string name);

        //auxiliares
        List<SecRoleDto> GetRoles();
    }
}
