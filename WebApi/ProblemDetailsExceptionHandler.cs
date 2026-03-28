using AppCore.Interfaces.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace WebApi;

public class ProblemDetailsExceptionHandler(
    ProblemDetailsFactory factory, ILogger<ProblemDetailsExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ContactNotFoundException || exception is ContactNoteNotFoundException)
        {
            logger.Log(LogLevel.Information, $"Exception '{exception.Message}' handled!");
            var problem = factory.CreateProblemDetails(
                context,
                StatusCodes.Status400BadRequest,
                "Contact service error!",
                "Service error",
                detail: exception.Message
            );
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problem);
            return true;
        }
        return false;
    }
}
