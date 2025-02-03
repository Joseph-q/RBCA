using AutoMapper;
using SistemaCorteDeCaja.Models;
using SistemaCorteDeCaja.Permissions.DTOs.Response;
using SistemaCorteDeCaja.Roles.Controllers.DTOs.Request;
using SistemaCorteDeCaja.Roles.Controllers.DTOs.Response;
using SistemaCorteDeCaja.Users.Controllers.DTOs.Request;
using SistemaCorteDeCaja.Users.Controllers.DTOs.Responses;

namespace SistemaCorteDeCaja
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // User Mapping

            // Hashes the password before mapping it to the PasswordHash field.
            CreateMap<CreateUserRequest, User>()
                .ForMember(d => d.PasswordHash, o => o.MapFrom(s => BCrypt.Net.BCrypt.HashPassword(s.Password)));

            // - If the password is empty, it keeps the current PasswordHash.
            // - If a password is provided, it generates a new hashed password.
            // - If the username is null, it keeps the current Username.
            CreateMap<UpdateUserRequest, User>()
                .ForMember(d => d.PasswordHash, o => o.MapFrom((s, d) =>
                    string.IsNullOrEmpty(s.Password) ? d.PasswordHash // If the password is empty, keep the current hash
                    : BCrypt.Net.BCrypt.HashPassword(s.Password) // If not, generate a new hash
                ))
                .ForMember(d => d.Username, o => o.MapFrom((s, d) => s.Username ?? d.Username)); // If username is null, keep the current one

            // Maps the user's role from the roles list, using the first role if available.
            CreateMap<User, UserDTO>();

            CreateMap<Role, UserRoleDTO>(); // Asegúrate de incluir esta línea

            //Role Mapping
            CreateMap<CreateRoleRequest, Role>();

            CreateMap<UpdateRoleRequest, Role>()
                .ForMember(d => d.Name, o => o.MapFrom((s, d) => s.Name ?? d.Name))
                .ForMember(d => d.Description, o => o.MapFrom((s, d) => s.Description ?? d.Description));

            CreateMap<Role, RoleDto>();


            //Permission Mapping
            CreateMap<Permission, PermissionDto>();
        }
    }
}
