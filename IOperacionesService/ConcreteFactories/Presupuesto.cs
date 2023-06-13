using AutoMapper;
using Erp.Api.CustomerService.Service;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.OperacionesService.Service;
using Erp.Api.SecurityService.Interfaces;

namespace Erp.Api.OperacionesService.ConcreteFactories
{
    public class Presupuesto : Operaciones
    {
        public Presupuesto(IUnitOfWork unitOfWork, ISecurityService security, ICustomer customer, IEstado estado, ITipoDoc tipoDoc, IMapper mapper) : base(unitOfWork, security, customer, estado, tipoDoc, mapper)
        {
        }

        public override Task Imprimir()
        {
            throw new NotImplementedException();
        }

    }
}
