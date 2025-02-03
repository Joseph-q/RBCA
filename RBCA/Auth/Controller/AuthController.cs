using Microsoft.AspNetCore.Mvc;
using SistemaCorteDeCaja.Auth.Controller.Request;
using SistemaCorteDeCaja.Auth.Services;
using SistemaCorteDeCaja.Models;
using SistemaCorteDeCaja.Shared.DTOs.Responses;

namespace SistemaCorteDeCaja.Auth.Controller
{
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    [ProducesResponseType(401)]
    [ProducesResponseType(404, Type = typeof(ErrorResponseDto))]
    public class AuthController(AuthService authservice) : ControllerBase
    {
        private readonly AuthService _authservice = authservice;

        [ProducesResponseType(200, Type = typeof(string))]
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
