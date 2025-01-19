using Microsoft.AspNetCore.Mvc;
using SistemaCorteDeCaja.Auth.Controller.Request;
using SistemaCorteDeCaja.Auth.Services;
using SistemaCorteDeCaja.Models;

namespace SistemaCorteDeCaja.Auth.Controller
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(AuthService authservice) : ControllerBase
    {
        private readonly AuthService _authservice = authservice;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestData login)
        {
            User? user = await _authservice.Login(login);

            if (user == null)
            {
                return Unauthorized();
            }

            string token = _authservice.GenerateJwtToken(user);

            return Ok(token);
        }
    }
}
