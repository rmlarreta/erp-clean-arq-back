using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Operaciones;
using Erp.Api.OperacionesService.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Web.Controllers
{
    public class RemitosController : CommonController
    {
        private readonly IOperacionesBusiness _operaciones;

        public RemitosController(IOperacionesBusiness operaciones)
        {
            _operaciones = operaciones;
        }

        [HttpPost]
        [ProducesResponseType(typeof(DataResponse<BusOperacionSumaryDto>), StatusCodes.Status201Created)]
        public async Task<IActionResult> NuevoRemito([FromBody] Request request)
        {
            return Ok(await _operaciones.NuevaOperacion("REMITO", request));
        }

        [HttpGet("{guid}")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Imprimir(Guid guid)
        {
            return await _operaciones.Imprimir(guid);
        }
    }
}
