using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SistemaCorteDeCaja.Auth.Controller.Request;
using SistemaCorteDeCaja.Models;
using SistemaCorteDeCaja.Shared.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace SistemaCorteDeCaja.Auth.Services
{
    public class AuthService
    {

        CorteDeCajaContext _context;
        private readonly JwtSettings _jwtSettings;


        public AuthService(CorteDeCajaContext context, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }


        public async Task<User?> Login(LoginRequestData login)
        {
            User? user = await GetUserWithPasswordAndRoles(login.Username);

            if (user == null)
            {
                return null;
            }

            if (!VerifyPassword(login.Password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        private Task<User?> GetUserWithPasswordAndRoles(string username)
        {
            return _context.Users
                .Include(user => user.Roles)
                .FirstOrDefaultAsync(user => user.Username == username);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }


        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var claims = new List<Claim>
            {
                new Claim("sub", user.Id.ToString()),
                new Claim("username", user.Username),
            };

            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationInHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
