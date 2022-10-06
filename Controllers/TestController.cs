using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OAuthTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var userClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            if (userClaim == null)
            {
                return Unauthorized();
            }

            var user = userClaim.Value;
            if (string.IsNullOrEmpty(user))
            {
                return Unauthorized();
            }
            return Ok(new { user });
        }
    }
}