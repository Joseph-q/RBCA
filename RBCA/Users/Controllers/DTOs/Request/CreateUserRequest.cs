using System.ComponentModel.DataAnnotations;

namespace SistemaCorteDeCaja.Users.Controllers.DTOs.Request
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres")]
        public string Username { get; set; }


        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(60, MinimumLength = 5, ErrorMessage = "La contraseña debe tener al menos 5 caracteres")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Rol no valido")]
        public int RoleId { get; set; }

    }
}
