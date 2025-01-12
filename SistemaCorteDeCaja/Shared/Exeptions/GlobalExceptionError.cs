using System.Net;

namespace SistemaCorteDeCaja.Shared.Exeptions
{
    public class GlobalExceptionError : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string Detail { get; }


        public GlobalExceptionError(HttpStatusCode statusCode, string message, string? detail = null)
           : base(message)
        {
            StatusCode = statusCode;
            Detail = detail;
        }

        // Constructor para manejar excepciones internas
        public GlobalExceptionError(HttpStatusCode statusCode, string message, string? detail, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            Detail = detail;
        }


    }
}
