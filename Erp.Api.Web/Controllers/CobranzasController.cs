using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Flow;
using Erp.Api.FlowService.Service;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Web.Controllers
{
    public class CobranzasController : CommonController
    {
        private readonly IRecibo _recibo;
        private readonly ITipoPagos _tipoPagos;
        private readonly IPos _pos;

        public CobranzasController(IRecibo recibo, ITipoPagos tipoPagos, IPos pos)
        {
            _recibo = recibo;
            _tipoPagos = tipoPagos;
            _pos = pos;
        }

        [HttpPost]
        [ProducesResponseType(typeof(DataResponse<Guid>), StatusCodes.Status201Created)]
        public async Task<IActionResult> NuevaCobranza([FromBody] CobReciboInsert recibo)
        {
            return Ok(await _recibo.InsertRecibo(recibo));
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
    }
}
