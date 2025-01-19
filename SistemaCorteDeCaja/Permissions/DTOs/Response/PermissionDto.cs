namespace SistemaCorteDeCaja.Permissions.DTOs.Response
{
    public class PermissionDto
    {
        public int Id { get; set; }

        public string Action { get; set; } = null!;

        public string Subject { get; set; } = null!;

        public string? Condition { get; set; }
    }
}
