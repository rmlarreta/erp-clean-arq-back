using Erp.Api.Domain.Entities;

namespace Erp.Api.SecurityService.Interfaces
{
    public interface IUserService
    {
        Task<SecUser> GetUserBaseByName(string username);
    }
}
