using SistemaCorteDeCaja.Shared.Exeptions;
using System.Net;

namespace SistemaCorteDeCaja.Roles.Exeptions
{
    public class RoleAlreadyExistException : GlobalExceptionError
    {
        public RoleAlreadyExistException(string message = "Role Already Exist", string? detail = null) : base(HttpStatusCode.BadRequest, message, detail)
        {
        }
    }
}
