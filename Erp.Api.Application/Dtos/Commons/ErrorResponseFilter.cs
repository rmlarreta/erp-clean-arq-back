using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Erp.Api.Application.Dtos.Commons
{
    public class ErrorResponseFilter : IExceptionFilter
    { 
        public void OnException(ExceptionContext context)
        {
            ErrorResponse errorResponse = new()
            {
                Message = context.Exception.Message,
                ErrorCode = context.Exception.HResult
            };

            DataResponse<ErrorResponse> response = new(errorResponse);
            context.Result = new ObjectResult(response)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}
