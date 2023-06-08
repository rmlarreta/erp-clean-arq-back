using Erp.Api.Application.Dtos.Customers;

namespace Erp.Api.CustomerService.Interfaces
{
    public interface ICustomerService
    {
        Task<OpCustomerDto> GetById(Guid id); 
        Task<OpCustomerDto> GetByCui(string cui);
    }
}
