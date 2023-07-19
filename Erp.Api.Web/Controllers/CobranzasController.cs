using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Flow;
using Erp.Api.FlowService.Business;
using Erp.Api.FlowService.Service;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Web.Controllers
{
    public class CobranzasController : CommonController
    {
        private readonly IRecibo _recibo;
        private readonly ITipoPagos _tipoPagos;
        private readonly IImputaciones _imputaciones;
        private readonly IPos _pos;

        public CobranzasController(IRecibo recibo, ITipoPagos tipoPagos, IImputaciones imputaciones, IPos pos)
        {
            _recibo = recibo;
            _tipoPagos = tipoPagos;
            _imputaciones = imputaciones;
            _pos = pos;
        }

        [HttpPost]
        [ProducesResponseType(typeof(DataResponse<Guid>), StatusCodes.Status201Created)]
        public async Task<IActionResult> NuevaCobranza([FromBody] CobReciboInsert recibo)
        {
            return Ok(await _recibo.InsertRecibo(recibo));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> SaldarPago([FromBody] Request request)
        {
            await _imputaciones.SaldarPago(request);
            return Ok(request.GuidRecibo);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> ImputarAlone([FromBody] Request request)
        {
            await _imputaciones.ImputarAlone(request);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(DataResponse<List<CobTipoPagoDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTipoPagos()
        {
            List<CobTipoPagoDto>? tipos = await _tipoPagos.GetTipoPagos();

            if (tipos == null)
            {
                return NoContent();
            }

            return Ok(tipos);
        }

        [HttpGet]
        [ProducesResponseType(typeof(DataResponse<List<PosDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPos()
        {
            List<PosDto>? pos = await _pos.GetPos();

            if (pos == null)
            {
                return NoContent();
            }

            return Ok(pos);
        }

        [HttpGet("{guid}")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Imprimir(Guid guid)
        {
            return await _recibo.Imprimir(guid);
        }
    }
}
