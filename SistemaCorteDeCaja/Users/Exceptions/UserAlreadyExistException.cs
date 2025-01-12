using SistemaCorteDeCaja.Shared.Exeptions;
using System.Net;

namespace SistemaCorteDeCaja.Users.Exeptions
{
    public class UserAlreadyExistException : GlobalExceptionError
    {
        public UserAlreadyExistException(string? message = null, string? detail = null) : base(HttpStatusCode.BadRequest, message ?? "User Already Exist", detail)
        {
        }
    }
}
