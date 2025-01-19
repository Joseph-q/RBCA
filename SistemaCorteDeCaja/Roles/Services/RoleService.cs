using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SistemaCorteDeCaja.Models;
using SistemaCorteDeCaja.Roles.Controllers.DTOs.Request;
using SistemaCorteDeCaja.Roles.Exeptions;

namespace SistemaCorteDeCaja.Roles.Services
{
    public class RoleService(CorteDeCajaContext contex, IMapper mapper)
    {
        private readonly CorteDeCajaContext _context = contex;
        private readonly IMapper _mapper = mapper;

        public async Task CreateRole(Role role)
        {
            _context.Add(role);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
            {
                throw new RoleAlreadyExistException();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public Task<Role?> GetRoleById(int id)
        {
            return _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public Task<List<Role>> GetRoles(GetRolesQueryParams queryParams)
        {
            int page = queryParams.Page;
            int limit = queryParams.Limit;
            return _context.Roles.Skip((page - 1)).Take(limit).ToListAsync();
        }

        public Task<int> UpdateRole(Role roleDb, UpdateRoleRequest roleToUpdate)
        {
            _mapper.Map(roleToUpdate, roleDb);

            try
            {
                return _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
            {
                throw new RoleAlreadyExistException();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public Task<int> DeleteRole(Role roleDb)
        {
            _context.Remove(roleDb);

            return _context.SaveChangesAsync();
        }


        public Task<int> AddPermissionsToRole(Role role, List<int> permissionsIds)
        {
            role.Permissions.Add((Permission)permissionsIds.Select(id => new Permission { Id = id }));

            return _context.SaveChangesAsync();
        }
    }
}
