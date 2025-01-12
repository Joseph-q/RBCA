namespace SistemaCorteDeCaja.Models;

public partial class RolePermission
{
    public int RoleId { get; set; }

    public string ObjectName { get; set; } = null!;

    public byte[]? Permissions { get; set; }

    public virtual Role Role { get; set; } = null!;
}
