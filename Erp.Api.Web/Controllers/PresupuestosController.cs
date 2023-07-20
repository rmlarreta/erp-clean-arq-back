using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Operaciones;
using Erp.Api.Application.Dtos.Operaciones.Detalles;
using Erp.Api.OperacionesService.BusinessLogic.Interfaces;
using Erp.Api.OperacionesService.Service;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Web.Controllers
{
    public class PresupuestosController : CommonController
    {
        private readonly IOperacionesBusiness _operaciones;
        private readonly IDetalles _detallesService;

        public PresupuestosController(IOperacionesBusiness operaciones, IDetalles detallesService)
        {
            _operaciones = operaciones;
            _detallesService = detallesService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(DataResponse<BusOperacionSumaryDto>), StatusCodes.Status201Created)]
        public async Task<IActionResult> NuevoPresupuesto()
        {
            return Ok(await _operaciones.NuevaOperacion("PRESUPUESTO",null));
        }

        [HttpGet]
        [ProducesResponseType(typeof(DataResponse<List<BusOperacionSumaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPresupuestos()
        {
            List<BusOperacionSumaryDto>? presupuestos = await _operaciones.GetAllOperaciones("PRESUPUESTO");

            if (!presupuestos.Any())
            {
                return NoContent();
            }

            return Ok(presupuestos);
        }

        [HttpGet("{guid}")]
        [ProducesResponseType(typeof(DataResponse<BusOperacionSumaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPresupuestoById(Guid guid)
        {
            BusOperacionSumaryDto? presupuesto = await _operaciones.GetOperacion("PRESUPUESTO", guid);

            if (presupuesto == null)
            {
                return NotFound();
            }

            return Ok(presupuesto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> InsertDetalles(List<BusOperacionDetalleDto> detalles)
        {
            await _detallesService.InsertDetalles(detalles);
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateDetalle(BusOperacionDetalleDto detalle)
        {
            await _detallesService.UpdateDetalle(detalle);
            return Ok();
        }

        [HttpDelete("{guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteDetalle(Guid guid)
        {
            await _detallesService.DeleteDetalle(guid);
            return Ok();
        }

        [HttpDelete("{guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeletePresupuesto(Guid guid)
        {
            await _operaciones.DeleteOperacion(guid);
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePresupuesto(BusOperacionInsert operacion)
        {
            await _operaciones.UpdateOperacion(operacion);
            return Ok();
        }

        [HttpGet("{guid}")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Imprimir(Guid guid)
        {
            return await _operaciones.Imprimir(guid);
        }
    }
}
