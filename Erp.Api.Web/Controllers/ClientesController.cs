using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Customers;
using Erp.Api.CustomerService.Service;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Web.Controllers
{
    public class ClientesController : CommonController
    {
        private readonly ICustomer _customer;

        public ClientesController(ICustomer customer)
        {
            _customer = customer;
        }
         
        [HttpGet]
        [ProducesResponseType(typeof(DataResponse<List<OpCustomerDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllClientes()
        {
            List<OpCustomerDto>? customers = await _customer.GetAllClientes();

            if (customers == null)
            {
                return NoContent();
            }

            return Ok(customers);
        }

        [HttpPost]
        [ProducesResponseType(typeof(DataResponse<>), StatusCodes.Status201Created)]
        public async Task<IActionResult> InsertCliente([FromBody] OpCustomerInsert customer)
        {
            await _customer.InsertCliente(customer);
            return CreatedAtAction(nameof(GetAllClientes), null);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DataResponse<>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCliente(Guid id)
        {
            await _customer.DeleteCliente(id);
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCliente([FromBody] OpCustomerInsert customer)
        {
            await _customer.UpdateCliente(customer);
            return Ok();
        }
    }
}
