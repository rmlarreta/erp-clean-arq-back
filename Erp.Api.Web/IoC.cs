using Erp.Api.Application.CommonService.Interfaces;
using Erp.Api.Application.CommonService.Services;
using Erp.Api.CustomerService.Interfaces;
using Erp.Api.Domain.Entities;
using Erp.Api.Domain.Repositories;
using Erp.Api.Domain.Services;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Repositories;
using Erp.Api.Infrastructure.Data.Services;
using Erp.Api.Infrastructure.UnitOfWorks;
using Erp.Api.OperacionesService.Application;
using Erp.Api.OperacionesService.Factories;
using Erp.Api.OperacionesService.Interfaces;
using Erp.Api.SecurityService.Interfaces;
using IOperacionesService.Application;
using IOperacionesService.Factory;
using IOperacionesService.Interfaces;

namespace Erp.Api.Web
{
    public static class IoC
    {
        public static void AddServices(this IServiceCollection services)
        {
            #region Factory
            services.AddScoped<DocumentosFactory>();
            services.AddScoped(typeof(Presupuesto<>));
            #endregion
            #region Commons
            services.AddScoped<ISystemIndexService, SystemIndexService>();
            services.AddScoped<ISysEmpresaService, SysEmpresaService>();
            #endregion
            #region Service
            services.AddScoped<ISecurityService, Erp.Api.SecurityService.Application.SecurityService>();
            services.AddScoped<IUserService, Erp.Api.SecurityService.Application.UserService>();
            services.AddScoped<IOperacionesServices, OperacionesServices>();
            services.AddScoped(typeof(IDocumentosService<>), typeof(DocumentosService<>));
            services.AddScoped<IEstadoService,EstadoService>();
            services.AddScoped<ITipoDocService, TipoDocService>();
            services.AddScoped<ICustomerService, Erp.Api.CustomerService.Application.CustomerService>();
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
