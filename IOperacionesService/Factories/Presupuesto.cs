using AutoMapper;
using Erp.Api.CustomerService.Service;
using Erp.Api.Domain.Entities;
using Erp.Api.Domain.Services;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.OperacionesService.Models;
using Erp.Api.OperacionesService.Service;
using Erp.Api.SecurityService.Interfaces;

namespace IOperacionesService.Factory
{
    public class Presupuesto<T> : OperacionTemplate<T> where T : class
    {
        public Presupuesto(IUnitOfWork unitOfWork, IService<BusOperacionDetalle> detalles, ISecurityService security, ICustomer customer, IEstado estado, ITipoDoc tipos, IMapper mapper) : base(unitOfWork, detalles, security, customer, estado, tipos, mapper)
        {
        }

        public override Task Imprimir()
        {
            // Lógica para imprimir un presupuesto
            // ...

            // Realiza la impresión
            return Task.CompletedTask;
        }
    }
}
