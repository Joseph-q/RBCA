using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SistemaCorteDeCaja.Shared.DTOs.Responses;
using SistemaCorteDeCaja.Shared.Exeptions;

namespace SistemaCorteDeCaja
{
    public class ExceptionHandlingFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionHandlingFilter> _logger;

        public ExceptionHandlingFilter(ILogger<ExceptionHandlingFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is GlobalExceptionError globalEx)
            {

                if (globalEx.InnerException != null)
                {
                    _logger.LogError(globalEx.InnerException, "Excepción interna relacionada.");
                }


                context.Result = new ObjectResult(new ErrorResponseDto()
                {
                    Title = globalEx.Message,
                    Detail = globalEx.Detail
                })
                {
                    StatusCode = (int)globalEx.StatusCode
                };

                context.ExceptionHandled = true;

                return;
            }

            _logger.LogError(context.Exception, "Internal Server Error");
            context.Result = new ObjectResult(new { message = "Internal Server Error" })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
            return;





        }
    }
}
