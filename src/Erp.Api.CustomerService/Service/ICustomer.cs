using Erp.Api.Application.Dtos.Customers;

namespace Erp.Api.CustomerService.Service
{
    public interface ICustomer
    {
        Task<OpCustomerDto> GetById(Guid id);

        Task<OpCustomerDto> GetByCui(string cui);

        Task<List<OpCustomerDto>> GetAllClientes();

        Task InsertCliente(OpCustomerInsert model);

        Task UpdateCliente(OpCustomerInsert model);

        Task DeleteCliente(Guid id);
    }
}
