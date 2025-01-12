namespace SistemaCorteDeCaja.Users.Controllers.DTOs.Responses
{
    public class UserDTO
    {
        public int Id { get; set; }
        public required string Username { get; set; }

        public DateTime? CreatedAt { get; set; }

        public required List<UserRoleDTO?> Roles { get; set; }
    }
}
