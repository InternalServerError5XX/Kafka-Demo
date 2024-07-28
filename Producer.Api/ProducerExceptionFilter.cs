using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Producer.Api.Services.KafkaProducerService;

namespace Producer.Api
{
    public class ProducerExceptionFilter(IKafkaProducerService producerService) : IAsyncExceptionFilter
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var errorMessage = new 
            { 
                Error = context.Exception.Message, 
                StackTrace = context.Exception.StackTrace 
            };
            await producerService.SendMessageAsync("books-topic", errorMessage, default);

            context.Result = new BadRequestObjectResult(errorMessage);
            context.ExceptionHandled = true;
        }
    }
}
