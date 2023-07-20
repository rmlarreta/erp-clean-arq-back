using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Erp.Api.Application.Dtos.Commons
{
    public class ResponseFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                object? data = objectResult.Value;

                // Modificar la respuesta si es necesario
                DataResponse<object> modifiedResponse = new(data);

                // Establecer la respuesta modificada
                context.Result = new ObjectResult(modifiedResponse)
                {
                    StatusCode = objectResult.StatusCode
                };
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // avoid
        }
    }
}

