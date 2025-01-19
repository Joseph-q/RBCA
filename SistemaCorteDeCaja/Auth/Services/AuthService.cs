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
    public class AuthService(CorteDeCajaContext context, IOptions<JwtSettings> jwtSettings)
    {

        private readonly CorteDeCajaContext _context = context;
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        //Authentication
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
                .Where(u => u.Username == username)
                .Include(user => user.Roles)
                .Select(u => new User
                {
                    Id = u.Id,
                    Username = u.Username,
                    PasswordHash = u.PasswordHash,
                    Roles = u.Roles.Select(r => new Role
                    {
                        Id = r.Id,
                        Name = r.Name
                    }).ToList(),
                })
                .FirstOrDefaultAsync();
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }


        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Roles.FirstOrDefault()?.Name ?? "no role")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationInHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        //Authorization
        public async Task<bool> CheckUserPermissionAsync(int userId, string roleName, string action, string subject)
        {
            User? user = await GetUserWithRole(userId, roleName);
            if (user == null || user.Roles.Count == 0) // Verifica si el usuario tiene roles
            {
                return false;
            }

            // Obtener permisos de todos los roles del usuario
            var permissions = new List<Permission>();
            foreach (var role in user.Roles)
            {
                var rolePermissions = await GetPermissionByRoleId(role.Id);
                permissions.AddRange(rolePermissions);
            }

            // Verificar si alguno de los permisos coincide con la acción y el sujeto
            foreach (var permission in permissions)
            {
                if (permission.Action == action && permission.Subject == subject)
                {
                    return true;
                }

                // Permiso de "manage" para "all" otorga acceso
                if (permission.Action == "manage" && permission.Subject == "all")
                {
                    return true;
                }
            }

            return false;
        }

        private Task<User?> GetUserWithRole(int userId, string rolename)
        {
            return _context.Users
                .Include(u => u.Roles) // Ensure roles are loaded
                .Where(u => u.Id == userId && u.Roles.Any(r => r.Name == rolename))
                .FirstOrDefaultAsync();
        }

        private Task<List<Permission>> GetPermissionByRoleId(int roleId)
        {
            return _context.Permissions
                .Where(p => p.Roles.Any(r => r.Id == roleId))
                .ToListAsync();
        }

    }
}
