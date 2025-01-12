using System.ComponentModel.DataAnnotations;

namespace SistemaCorteDeCaja.Users.Controllers.DTOs.Request
{
    public class GetUserQueryParams
    {
        [Range(1, int.MaxValue, ErrorMessage = "El límite debe ser mayor o igual a 1.")]
        public int? RoleLimit { get; set; }
    }
}
