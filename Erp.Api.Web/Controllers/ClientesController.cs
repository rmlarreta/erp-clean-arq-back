using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Customers;
using Erp.Api.CustomerService.Business;
using Erp.Api.CustomerService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Web.Controllers
{ 
    public class ClientesController : CommonController
    {
        private readonly ICustomer _customer;
        private readonly ICustomerBusiness _customerBusiness;

        public ClientesController(ICustomer customer, ICustomerBusiness customerBusiness)
        {
            _customer = customer;
            _customerBusiness = customerBusiness;
        }

        [HttpGet]
        [ProducesResponseType(typeof(DataResponse<List<OpCustomerDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllClientes()
        {
            List<OpCustomerDto>? customers = await _customer.GetAllClientes();

            if (!customers.Any())
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

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DataResponse<CustomerConciliacion>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConciliacion(Guid id)
        {
            CustomerConciliacion? conciliacion = await _customerBusiness.GetConciliacion(id);

            if (conciliacion == null)
            {
                return NoContent();
            }

            return Ok(conciliacion);
        }
    }
}
