using Erp.Api.Domain.Entities;
using Erp.Api.Domain.Repositories;
using Erp.Api.Domain.Services;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Repositories;
using Erp.Api.Infrastructure.Data.Services;
using Erp.Api.Infrastructure.Helpers;
using Erp.Api.Infrastructure.UnitOfWorks;
using Erp.Api.SecurityService.Interfaces;

namespace Erp.Api.Web
{
    public static class IoC
    {
        public static void AddServices(this IServiceCollection services)
        {
            
            #region Service
            services.AddScoped<ISecurityService, Erp.Api.SecurityService.Application.SecurityService>();
            services.AddScoped<IUserService, Erp.Api.SecurityService.Application.UserService>();
            #endregion Service
            #region Repositories
            services.AddScoped(typeof(IService<>), typeof(Service<>));
            services.AddScoped<IEntity, Entity>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            #endregion Repositories

        }
    }
}
