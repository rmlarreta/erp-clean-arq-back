using Erp.Api.Application.CommonService.Interfaces;
using Erp.Api.Application.CommonService.Services;
using Erp.Api.CustomerService.Service;
using Erp.Api.Domain.Entities;
using Erp.Api.Domain.Repositories;
using Erp.Api.Domain.Services;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Repositories;
using Erp.Api.Infrastructure.Data.Services;
using Erp.Api.Infrastructure.UnitOfWorks;
using Erp.Api.OperacionesService.BusinessLogic.Application;
using Erp.Api.OperacionesService.BusinessLogic.Interfaces;
using Erp.Api.OperacionesService.ConcreteFactories;
using Erp.Api.OperacionesService.Service;
using Erp.Api.ProductosService.Service;
using Erp.Api.SecurityService.Interfaces;

namespace Erp.Api.Web
{
    public static class IoC
    {
        public static void AddServices(this IServiceCollection services)
        {
            #region Factory

            services.AddScoped<Presupuesto>();
            services.AddScoped<ConcreteOperacion>();
            #endregion

            #region Commons
            services.AddScoped<ISystemIndexService, SystemIndexService>();
            services.AddScoped<ISysEmpresaService, SysEmpresaService>();
            #endregion



            #region Service
            services.AddScoped<ISecurityService, Erp.Api.SecurityService.Application.SecurityService>();
            services.AddScoped<IUserService, Erp.Api.SecurityService.Application.UserService>();
            services.AddScoped<IOperacionesBusiness, OperacionesBusiness>(); 
            services.AddScoped<IDetalles, Detalles>();
            services.AddScoped<IEstado, Estado>();
            services.AddScoped<ITipoDoc, TipoDoc>();
            services.AddScoped<ICustomer, Customer>();
            services.AddScoped<ICommons, Commons>();
            services.AddScoped<IProductos, Productos>();
            services.AddScoped<IRubros, Rubros>();
            services.AddScoped<IProductosIva, ProductosIva>();
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
