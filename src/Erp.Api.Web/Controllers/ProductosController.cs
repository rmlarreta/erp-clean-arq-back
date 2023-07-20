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

            if (!productos.Any())
            {
                return NoContent();
            }

            return Ok(productos);
        }

        [HttpPost]
        [ProducesResponseType(typeof(DataResponse<>),StatusCodes.Status201Created)]
        public async Task<IActionResult> InsertProducto([FromBody] ProductoDto producto)
        {
            await _productos.InsertProducto(producto);
            return CreatedAtAction(nameof(GetAllProductos), null);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DataResponse<>),StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteProducto(Guid id)
        {
            await _productos.DeleteProducto(id);
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProducto([FromBody] ProductoDto producto)
        {
            await _productos.UpdateProducto(producto);
            return Ok();
        }
    }
}
