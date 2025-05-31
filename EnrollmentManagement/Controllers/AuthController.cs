using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EnrollmentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            if (dto.Username == "usuario" && dto.Password == "123")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, dto.Username)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("esta_es_una_clave_muy_segura_y_larga_1234"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds,
                    claims: claims
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return Unauthorized("Usuario o contraseña incorrectos");
        }
    }

    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
