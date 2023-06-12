using Erp.Api.Application.CommonService.Interfaces;
using Erp.Api.Application.CommonService.Services;
using Erp.Api.Application.Dtos.Operaciones;
using Erp.Api.CustomerService.Service;
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
using Erp.Api.OperacionesService.Models;
using Erp.Api.OperacionesService.Service;
using Erp.Api.ProductosService;
using Erp.Api.ProductosService.Service;
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
            services.AddScoped<OperacionTemplate<BusOperacion>, Presupuesto<BusOperacion>>();
            services.AddScoped<OperacionTemplate<BusOperacionDto>, Presupuesto<BusOperacionDto>>();
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
            services.AddScoped<IDetalles, Detalles>();
            services.AddScoped<IEstado, Estado>();
            services.AddScoped<ITipoDoc, TipoDoc>();
            services.AddScoped<ICustomer, Customer>();
            services.AddScoped<IProductos, Productos>();
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
