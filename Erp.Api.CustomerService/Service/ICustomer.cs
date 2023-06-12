using Erp.Api.Application.Dtos.Customers;

namespace Erp.Api.CustomerService.Service
{
    public interface ICustomer
    {
        Task<OpCustomerDto> GetById(Guid id);
        Task<OpCustomerDto> GetByCui(string cui);
    }
}
