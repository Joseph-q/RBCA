namespace SistemaCorteDeCaja.Auth.Atributtes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class PermissionPolicy(string action, string subject) : Attribute
    {
        public string Action { get; } = action;
        public string Subject { get; } = subject;
    }
}
