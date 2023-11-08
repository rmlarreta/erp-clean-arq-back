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

        [HttpGet]
        [ProducesResponseType(typeof(DataResponse<List<OpConciliacionProviders>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPendientes()
        {
            List<OpConciliacionProviders>? documentos = await _providers.GetDocumentosPendientes();

            return Ok(documentos);
        }

        [HttpPost]
        [ProducesResponseType(typeof(DataResponse<>), StatusCodes.Status201Created)]
        public async Task<IActionResult> InsertDocumento([FromBody] OpDocumentoProveedorInsert documento)
        {
            await _providers.AltaDocumento(documento);
            return CreatedAtAction(nameof(GetAllPendientes), null);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(DataResponse<>), StatusCodes.Status201Created)]
        public async Task<IActionResult> Pago([FromBody] OpPagoProveedor pago)
        {
            await _providers.Pago(pago);
            return CreatedAtAction(nameof(GetAllPendientes), null);
        }
    }
}
