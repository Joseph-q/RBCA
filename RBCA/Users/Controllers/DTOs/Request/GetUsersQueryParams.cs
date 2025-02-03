using System.ComponentModel.DataAnnotations;

namespace SistemaCorteDeCaja.Users.Controllers.DTOs.Request
{
    public class GetUsersQueryParams
    {
        [Range(1, int.MaxValue, ErrorMessage = "El número de página debe ser mayor o igual a 1.")]
        public int Page { get; set; } = 1;


        [Range(1, int.MaxValue, ErrorMessage = "El límite debe ser mayor o igual a 1.")]
        public int Limit { get; set; } = 20;

        [Range(1, int.MaxValue, ErrorMessage = "El límite debe ser mayor o igual a 1.")]
        public int? RoleId { get; set; }
    }
}
