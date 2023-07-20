using AutoMapper;
using Erp.Api.Application.CommonService.Interfaces;
using Erp.Api.CustomerService.Service;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.OperacionesService.Service;
using Erp.Api.SecurityService.Interfaces;

namespace Erp.Api.OperacionesService.ConcreteFactories
{
    public class Presupuesto : Operaciones
    {
        public Presupuesto(IUnitOfWork unitOfWork, ISysEmpresaService empresa, ISecurityService security, ICustomer customer, IEstado estado, ITipoDoc tipos, IMapper mapper) : base(unitOfWork, empresa, security, customer, estado, tipos, mapper)
        {
        }
    }
}
