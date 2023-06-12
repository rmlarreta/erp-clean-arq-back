using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Productos;
using Erp.Api.ProductosService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Web.Controllers
{
    public class ProductosController : CommonController
    {
        private readonly IProductos _productos;

        public ProductosController(IProductos productos)
        {
            _productos = productos;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(DataResponse<List<ProductoSummaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProductos()
        {
            List<ProductoSummaryDto>? productos = await _productos.Listado();

            if (productos == null)
            {
                return NoContent();
            }

            return Ok(productos);
        }
    }
}
