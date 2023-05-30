using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Users;
using Erp.Api.SecurityService.Extensions;
using Erp.Api.SecurityService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Web.Controllers
{
    public class UsersController : CommonController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut]
        [Authorize(Policy = Policies.Admin)]
        public async Task<IActionResult> CreateUser([FromBody] UserInsertDto userInsert)
        {
            await _userService.CreateUser(userInsert);
            return Ok("Usuario Creado Correctamente");
        }

        [HttpPatch]
        [Authorize(Policy = Policies.Admin)]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto userUpdate)
        {
            await _userService.UpdateUser(userUpdate);
            return Ok("Usuario Actualizado Correctamente");
        }

        [HttpDelete("Id")]
        [Authorize(Policy = Policies.Admin)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _userService.DeleteUser(Guid.Parse(id));
            return Ok("Usuario Eliminado Correctamente");
        }

        [HttpGet]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(typeof(DataResponse<List<UserDto>>), StatusCodes.Status200OK)]
        public IActionResult GetAllUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet("Id")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(typeof(DataResponse<UserDto>), StatusCodes.Status200OK)]
        public IActionResult GetUserById(string id)
        {
            return Ok(_userService.GetUserById(Guid.Parse(id)));
        }

        [HttpGet("Name")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(typeof(DataResponse<UserDto>), StatusCodes.Status200OK)]
        public IActionResult GetUserByName(string name)
        {
            return Ok(_userService.GetUserByName(name));
        }
    }
}
