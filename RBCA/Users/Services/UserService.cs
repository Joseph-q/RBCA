using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SistemaCorteDeCaja.Models;
using SistemaCorteDeCaja.Roles.Exceptions;
using SistemaCorteDeCaja.Users.Controllers.DTOs.Request;
using SistemaCorteDeCaja.Users.Exeptions;

namespace SistemaCorteDeCaja.Users.Services
{
    public class UserService(CorteDeCajaContext context, IMapper mapper)
    {
        private readonly IMapper _mapper = mapper;
        private readonly CorteDeCajaContext _context = context;

        public async Task CreateUser(CreateUserRequest user)
        {
            var roleTask = _context.Roles.FirstOrDefaultAsync(r => r.Id == user.RoleId);

            User userToCreate = _mapper.Map<User>(user);

            var role = await roleTask;
            if (role == null)
            {
                throw new RoleNotFoundException();
            }

            userToCreate.Roles.Add(role);
            _context.Add(userToCreate);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
            {
                throw new UserAlreadyExistException();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public Task<List<User>> GetUsers(GetUsersQueryParams queryParams)
        {
            int page = queryParams.Page;
            int limit = queryParams.Limit;
            int? roleId = queryParams.RoleId;
            var chain = _context.Users.AsQueryable()
                .Select(u => new User
                {
                    Id = u.Id,
                    Username = u.Username,
                    CreatedAt = u.CreatedAt,
                });

            if (roleId != null && roleId > 0)
            {
                chain = chain.Where(u => u.Roles.Any(role => role.Id == queryParams.RoleId));
            }

            return chain.Skip((page - 1) * limit).Take(limit)
                .OrderBy(u => u.CreatedAt)
                .ToListAsync();
        }


        // Versión que acepta solo el 'id'
        public Task<User?> GetUserById(int id)
        {
            return _context.Users
                .Where(u => u.Id == id)
                .Select(u => new User
                {
                    Id = u.Id,
                    CreatedAt = u.CreatedAt,
                    Username = u.Username,
                })
                .FirstOrDefaultAsync();
        }

        // Versión que acepta tanto el 'id' como 'queryParams'
        public Task<User?> GetUserById(int id, GetUserQueryParams queryParams)
        {
            var chain = _context.Users.AsQueryable().Select(u => new User
            {
                Id = u.Id,
                Username = u.Username,
                CreatedAt = u.CreatedAt,
            });

            int? roleLimit = queryParams.RoleLimit;

            if (roleLimit != null && roleLimit > 0)
            {
                chain = chain.Select(u => new User
                {
                    Id = u.Id,
                    Username = u.Username,
                    Roles = u.Roles
                        .Take((int)roleLimit)
                        .Select(role => new Role
                        {
                            Id = role.Id,
                            Name = role.Name,
                            Description = role.Description
                        })
                        .ToList() // Convertir a lista de Role
                });
            }

            return chain.FirstOrDefaultAsync(u => u.Id == id);
        }


        public async Task UpdateUser(User userDb, UpdateUserRequest userToUpdate)
        {
            if (userToUpdate.RoleId != null)
            {
                var role = _context.Roles.FirstOrDefault(r => r.Id == userToUpdate.RoleId);
                if (role != null)
                {
                    userDb.Roles.Clear();
                    userDb.Roles.Add(role);
                }
            }

            _mapper.Map(userToUpdate, userDb);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
            {
                throw new UserAlreadyExistException();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }




        public async Task DeleteUser(User user)
        {
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }

    }
}
