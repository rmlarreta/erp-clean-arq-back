using Erp.Api.Application.Dtos.Customers;

namespace Erp.Api.CustomerService.Business
{
    public interface ICustomerBusiness
    {
        Task<CustomerConciliacion> GetConciliacion(Guid CustomerId);
    }
}

