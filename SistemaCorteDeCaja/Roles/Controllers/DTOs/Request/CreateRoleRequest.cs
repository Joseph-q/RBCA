using System.ComponentModel.DataAnnotations;

namespace SistemaCorteDeCaja.Roles.Controllers.DTOs.Request
{
    public class CreateRoleRequest
    {
        [Required]
        [MaxLength(20)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Description { get; set; }
    }

}
