namespace SistemaCorteDeCaja.Users.Controllers.DTOs.Responses
{
    public class UserRoleDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

    }
}
