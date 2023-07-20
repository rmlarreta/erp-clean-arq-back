using Erp.Api.Application.Dtos.Customers;
using Erp.Api.Application.Dtos.Operaciones;

namespace Erp.Api.CustomerService.Business
{
    public interface ICustomerBusiness
    {
        Task<CustomerConciliacion> GetConciliacion(Guid CustomerId);

        Task<IList<BusOperacionSumaryDto>> OperacionesImpagas(Guid CustomerId);
    }
}

