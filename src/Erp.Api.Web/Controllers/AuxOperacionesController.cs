using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Operaciones.Commons;
using Erp.Api.OperacionesService.Service;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Web.Controllers
{
    public class AuxOperacionesController : CommonController
    {
        private readonly ITipoDoc _tipos;

        public AuxOperacionesController(ITipoDoc tipos)
        {
            _tipos = tipos;
        }

        [HttpGet]
        [ProducesResponseType(typeof(DataResponse<List<TipoOperacionDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTipos()
        {
            List<TipoOperacionDto>? tipos = await _tipos.GetAll(); 

            if (!tipos.Any())
            {
                return NoContent();
            }

            return Ok(tipos);
        } 

    }
}
