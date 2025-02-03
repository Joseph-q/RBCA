namespace RBCA.Users.Controllers.DTOs.Responses
{
    public class UserWithoutRolesDTO
    {
        public int Id { get; set; }
        public required string Username { get; set; }

        public DateTime? CreatedAt { get; set; }

    }
}
