using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Web.Controllers
{
    [Authorize]
    [Route("api/[Controller]/[Action]")]
    [ApiController]
    public abstract class CommonController : ControllerBase

    {

    }
}
