using Erp.Api.Application.Dtos.Commons;
using Erp.Api.OperacionesService.Interfaces;
using Erp.Api.Application.Dtos.Operaciones;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Web.Controllers
{
   
    public class OperacionesController : CommonController
    {
        private readonly IOperacionesServices _operacionesService;

        public OperacionesController(IOperacionesServices operacionesService)
        {
            _operacionesService = operacionesService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(DataResponse<BusOperacionSumaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> NuevoPresupuesto()
        {
            return Ok(await _operacionesService.NuevoPresupuesto());
        }
    }
}
