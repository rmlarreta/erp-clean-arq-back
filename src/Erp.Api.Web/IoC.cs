﻿using Erp.Api.Application.CommonService.Interfaces;
using Erp.Api.Application.CommonService.Services;
using Erp.Api.CustomerService.Business;
using Erp.Api.CustomerService.Service;
using Erp.Api.Domain.Entities;
using Erp.Api.Domain.Repositories;
using Erp.Api.Domain.Services;
using Erp.Api.FlowService.Business;
using Erp.Api.FlowService.Service;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Repositories;
using Erp.Api.Infrastructure.Data.Services;
using Erp.Api.Infrastructure.UnitOfWorks;
using Erp.Api.OperacionesService.BusinessLogic.Application;
using Erp.Api.OperacionesService.BusinessLogic.Interfaces;
using Erp.Api.OperacionesService.ConcreteFactories;
using Erp.Api.OperacionesService.Service;
using Erp.Api.ProductosService.Service;
using Erp.Api.ProviderService.BusinessLogic;
using Erp.Api.ProviderService.Services;
using Erp.Api.SecurityService.Interfaces;

namespace Erp.Api.Web
{
    public static class IoC
    {
        public static void AddServices(this IServiceCollection services)
        {
            #region Factory
            services.AddScoped<Remito>();
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
            services.AddScoped<IRecibo, Recibo>();
            services.AddScoped<ITipoPagos, TipoPagos>();
            services.AddScoped<IPos, Pos>();
            services.AddScoped<IImputaciones, Imputaciones>();
            services.AddScoped<ICustomerBusiness, CustomerBusiness>();
            services.AddScoped<IProvidersBusiness, ProvidersBusiness>();
            services.AddScoped<IDocumentoProveedorService, DocumentoProveedorService>();
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
