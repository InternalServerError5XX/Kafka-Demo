using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Consumer.Api
{
    public class ConsumerExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var errorMessage = new 
            { 
                Error = context.Exception.Message, 
                StackTrace = context.Exception.StackTrace 
            };

            context.Result = new BadRequestObjectResult(errorMessage);
            context.ExceptionHandled = true;
        }
    }
}
