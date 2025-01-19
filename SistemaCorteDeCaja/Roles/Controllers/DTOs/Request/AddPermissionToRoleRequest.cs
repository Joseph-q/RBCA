using System.ComponentModel.DataAnnotations;

namespace SistemaCorteDeCaja.Roles.Controllers.DTOs.Request
{
    public class AddPermissionToRoleRequest
    {
        [Required]
        public readonly List<int> permissionsId;
    }
}
