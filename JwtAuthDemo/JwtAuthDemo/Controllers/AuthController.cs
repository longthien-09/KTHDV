using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly string _secretKey = "YourSecretKeyHere!"; // Đảm bảo bảo mật thực tế khi triển khai

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            // Kiểm tra tài khoản và mật khẩu (trong thực tế sẽ phải có cơ chế kiểm tra cơ sở dữ liệu)
            if (model.UserName == "user" && model.Password == "password")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    // Thêm các claim khác nếu cần
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds,
                    claims: claims
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return Unauthorized();
        }

        [Authorize]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Hello World");
        }
    }

    public class LoginModel
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
