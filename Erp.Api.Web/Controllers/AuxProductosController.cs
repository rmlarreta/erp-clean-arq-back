using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Productos;
using Erp.Api.ProductosService.Service;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Web.Controllers
{
    public class AuxProductosController : CommonController
    {
        private readonly IRubros _rubros;
        private readonly IProductosIva _iva;
        public AuxProductosController(IRubros rubros, IProductosIva iva)
        {
            _rubros = rubros;
            _iva = iva;
        }

        [HttpGet]
        [ProducesResponseType(typeof(DataResponse<List<RubroDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRubros()
        {
            List<RubroDto>? rubros = await _rubros.GetAllRubros();

            if (rubros == null)
            {
                return NoContent();
            }

            return Ok(rubros);
        }

        [HttpGet]
        [ProducesResponseType(typeof(DataResponse<List<IvaDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllIvas()
        {
            List<IvaDto>? rubros = await _iva.GetIvas();

            if (rubros == null)
            {
                return NoContent();
            }

            return Ok(rubros);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> InsertRubro([FromBody] RubroDto rubro)
        {
            await _rubros.InsertRubro(rubro);
            return CreatedAtAction(nameof(GetAllRubros), null);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DataResponse<>),StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteRubro(Guid id)
        {
            await _rubros.DeleteRubro(id);
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateRubro([FromBody] RubroDto rubro)
        {
            await _rubros.UpdateRubro(rubro);
            return Ok();
        }

    }
}
