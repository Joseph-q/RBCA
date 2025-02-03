using System.ComponentModel.DataAnnotations;

namespace SistemaCorteDeCaja.Roles.Controllers.DTOs.Request
{
    public class GetRolesQueryParams
    {
        [Range(1, int.MaxValue, ErrorMessage = "El número de página debe ser mayor o igual a 1.")]
        public int Page { get; set; } = 1;


        [Range(1, int.MaxValue, ErrorMessage = "El límite debe ser mayor o igual a 1.")]
        public int Limit { get; set; } = 20;
    }
}
