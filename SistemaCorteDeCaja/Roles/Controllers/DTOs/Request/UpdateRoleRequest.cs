using System.ComponentModel.DataAnnotations;

namespace SistemaCorteDeCaja.Roles.Controllers.DTOs.Request
{
    public class UpdateRoleRequest
    {
        [MaxLength(20)]
        public string? Name { get; set; }

        [MaxLength(50)]
        public string? Description { get; set; }
    }
}
