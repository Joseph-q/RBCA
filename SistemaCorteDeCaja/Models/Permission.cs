using System;
using System.Collections.Generic;

namespace SistemaCorteDeCaja.Models;

public partial class Permission
{
    public int Id { get; set; }

    public string Action { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string? Condition { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
