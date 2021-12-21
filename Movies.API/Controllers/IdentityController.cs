using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetIdentity()
        {
            var claims = HttpContext.User.Claims.Select(c => new { c.Type, c.Value });

            return new JsonResult(claims);
        }
    }
}
