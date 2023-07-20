using Erp.Api.Application.Dtos.Security;

namespace Erp.Api.SecurityService.Interfaces
{
    public interface ISecurityService
    {
        Task<UserAuth> Authenticate(UserRequest userRequest);
        Task<UserAuth> ChangePassword(UserRequest userRequest);
        public string GetUserAuthenticated();
        public string GetPerfilAuthenticated();
    }
}
