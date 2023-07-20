using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Providers;
using Erp.Api.ProviderService.BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Web.Controllers
{
    public class ProveedoresController : CommonController
    {
        private readonly IProvidersBusiness _providers;

        public ProveedoresController(IProvidersBusiness providers)
        {
            _providers = providers;
        }
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(DataResponse<List<OpDocumentoProveedorDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPendientes()
        {
            List<OpDocumentoProveedorDto>? documentos = await _providers.GetDocumentosPendientes();

            if (!documentos.Any())
            {
                return NoContent();
            }

            return Ok(documentos);
        }
    }
}
