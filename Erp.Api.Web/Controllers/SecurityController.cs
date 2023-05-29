using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Security;
using Erp.Api.SecurityService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Web.Controllers
{
    public class SecurityController : CommonController
    {
        private readonly ISecurityService _securityService;

        public SecurityController(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        [AllowAnonymous]
        [HttpPut]
        [ProducesResponseType(typeof(DataResponse<UserAuth>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Authenticate([FromBody] UserRequest userRequest)
        {
            return Ok(await _securityService.Authenticate(userRequest));
        }

        [AllowAnonymous]
        [HttpPut]
        [ProducesResponseType(typeof(DataResponse<UserAuth>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangePassword([FromBody] UserRequest userRequest)
        {
            return Ok(await _securityService.ChangePassword(userRequest));
        }
    }
}
