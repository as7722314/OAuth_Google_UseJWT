using Microsoft.IdentityModel.Tokens;
using OAuthTest.Models.GoogleOAuth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OAuthTest.Helpers
{
    public class JwtHelpers
    {
        private readonly IConfiguration _configuration;

        public JwtHelpers(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Userinfo user, string role = "user", int expireMinutes = 30)
        {
            var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email), //HttpContext.User.Identity.Name
            new Claim(ClaimTypes.Role, role), //HttpContext.User.IsInRole("r_admin")
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, role),
            //new Claim("Username", user.Name),
            //new Claim("Name", "超級管理員")
        };

            // 2. 從 appsettings.json 中讀取SecretKey
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            // 3. 選擇加密演算法
            var algorithm = SecurityAlgorithms.HmacSha256;

            // 4. 生成Credentials
            var signingCredentials = new SigningCredentials(secretKey, algorithm);

            // 5. 根據以上，生成token
            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],     //Issuer
                _configuration["Jwt:Audience"],   //Audience
                claims,                          //Claims,
                DateTime.Now,                    //notBefore
                DateTime.Now.AddMinutes(expireMinutes),    //expires
                signingCredentials               //Credentials
            );

            // 6. 將token變為string
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }
    }
}