using Erp.Api.Application.Dtos.Commons;
using Erp.Api.CustomerService.Service;
using Erp.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Web.Controllers
{
    public class AuxClientesController : CommonController
    {
        private readonly ICommons _commons;

        public AuxClientesController(ICommons commons)
        {
            _commons = commons;
        }

        [HttpGet]
        [ProducesResponseType(typeof(DataResponse<List<OpGender>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllGenders()
        {
            List<OpGender>? genders = await _commons.Genders();

            if (genders == null)
            {
                return NoContent();
            }

            return Ok(genders);
        }

        [HttpGet]
        [ProducesResponseType(typeof(DataResponse<List<OpPai>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPaises()
        {
            List<OpPai>? paises = await _commons.Paises();

            if (paises == null)
            {
                return NoContent();
            }

            return Ok(paises);
        }

        [HttpGet]
        [ProducesResponseType(typeof(DataResponse<List<OpResp>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllResps()
        {
            List<OpResp>? resps = await _commons.Resps();

            if (resps == null)
            {
                return NoContent();
            }

            return Ok(resps);
        }
    }
}
