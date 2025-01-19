using Microsoft.EntityFrameworkCore;
using SistemaCorteDeCaja.Models;
using SistemaCorteDeCaja.Permissions.DTOs.Request;

namespace SistemaCorteDeCaja.Permissions
{
    public class PermissionService(CorteDeCajaContext context)
    {
        private readonly CorteDeCajaContext _context = context;

        public Task<List<Permission>> GetPermissions(GetPermissionsQueryParams queryParams)
        {
            int page = queryParams.Page;
            int limit = queryParams.Limit;
            string? subject = queryParams.Subject;

            var chain = _context.Permissions.AsQueryable()
                .Select(p => new Permission { Action = p.Action, Subject = p.Subject });

            if (!string.IsNullOrEmpty(subject))
            {
                chain = chain.Where(p => p.Subject.Equals(subject));
            }

            return chain
                .Skip((page - 1)).Take(limit)
                .ToListAsync();
        }
    }
}
