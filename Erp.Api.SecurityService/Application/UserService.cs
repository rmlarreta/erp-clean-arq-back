using Erp.Api.Domain.Entities;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using Erp.Api.SecurityService.Interfaces;
using System.Linq.Expressions;

namespace Erp.Api.SecurityService.Application
{
    public class UserService : Service<SecUser>, IUserService
    {
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<SecUser> GetUserBaseByName(string username)
        {
            Expression<Func<SecUser, bool>> expression = c => c.UserName == username;
            Expression<Func<SecUser, object>>[] includeProperties = new Expression<Func<SecUser, object>>[]
            {
                    u=>u.RoleNavigation
            };
            return await Get(expression, includeProperties);
        }
    }
}
