using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using OAuthTest.Helpers;
using OAuthTest.Models.GoogleOAuth;
using OAuthTest.Services;
using System.Text;

namespace OAuthTest.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly OIDCService _oidcService;
        private readonly JwtHelpers _jwtHelpers;
        private readonly IConfiguration _configuration;

        public AuthController(OIDCService oidcService, JwtHelpers jwtHelpers, IConfiguration configuration)
        {
            _oidcService = oidcService;
            _jwtHelpers = jwtHelpers;
            _configuration = configuration;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            string host = "https://accounts.google.com/o/oauth2/v2/auth";
            StringBuilder StrParam = new();
            StrParam.Append("scope=https://www.googleapis.com/auth/userinfo.email&");
            StrParam.Append($"client_id={_configuration["Google:ClientId"]}&");
            StrParam.Append("redirect_uri=https://localhost:7228/api/auth/oidc/signin&");
            StrParam.Append("response_type=code&");
            StrParam.Append("access_type=offline&");
            StrParam.Append("state=state_parameter_passthrough_value");

            return Redirect(host + "?" + StrParam.ToString());
        }

        [HttpGet("oidc/signin")]
        public async Task<IActionResult> SigninOIDCAsync(string code, string state, string? error)
        {
            string idToken = await _oidcService.GetIdTokenAsync(code);
            var userInfo = await _oidcService.GetTokenInfo(idToken);
            if (userInfo is not null)
            {
                if (userInfo.Hd.Contains("cloudysys.com"))
                {
                    var jwtToken = _jwtHelpers.GenerateToken(userInfo, "admin");
                    return Ok(new { token = jwtToken });
                }
            }
            return Unauthorized();
        }
    }
}