using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Operaciones;
using Erp.Api.Application.Dtos.Operaciones.Detalles;
using Erp.Api.OperacionesService.Interfaces;
using Erp.Api.OperacionesService.Service;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Web.Controllers
{
    public class PresupuestosController : CommonController
    {
        private readonly IOperacionesServices _operacionesService;
        private readonly IDetalles _detallesService;

        public PresupuestosController(IOperacionesServices operacionesService, IDetalles detallesService)
        {
            _operacionesService = operacionesService;
            _detallesService = detallesService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(DataResponse<BusOperacionSumaryDto>), StatusCodes.Status201Created)]
        public async Task<IActionResult> NuevoPresupuesto()
        {
            return Ok(await _operacionesService.NuevoPresupuesto());
        }

        [HttpGet]
        [ProducesResponseType(typeof(DataResponse<List<BusOperacionSumaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPresupuestos()
        {
            List<BusOperacionSumaryDto>? presupuestos = await _operacionesService.GetAllPresupuestos();

            if (presupuestos == null)
            {
                return NoContent();
            }

            return Ok(presupuestos);
        }

        [HttpGet("{guid}")]
        [ProducesResponseType(typeof(DataResponse<BusOperacionSumaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPresupuestoById(Guid guid)
        {
            BusOperacionSumaryDto? presupuesto = await _operacionesService.GetOperacion(guid);

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
    }
}
