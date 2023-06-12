using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Erp.Api.Application.Dtos.Commons
{
    public class ErrorResponseFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            ErrorResponse errorResponse = new()
            {
                Message = context.Exception.Message + context.Exception.InnerException ?? $"({context.Exception.InnerException!.Message})",
                ErrorCode = context.Exception.GetType().Name switch
                {
                    "UnauthorizedAccessException" => 400,
                    _ => 500
                }
            };

            DataResponse<ErrorResponse> response = new(errorResponse);
            context.Result = new ObjectResult(response)
            {

                StatusCode = context.Exception.GetType().Name switch
                {
                    "UnauthorizedAccessException" => 400,
                    _ => 500
                }
            };

            context.ExceptionHandled = true;
        }
    }
}
