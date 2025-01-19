using Microsoft.EntityFrameworkCore;
using SistemaCorteDeCaja.Models;

namespace SistemaCorteDeCaja.Authorization.Services
{
    public class AuthorizationService(CorteDeCajaContext context)
    {
        private readonly CorteDeCajaContext _context = context;

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
    }
}
