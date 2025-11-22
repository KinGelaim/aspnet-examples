using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ExceptionHandlerMiddleware.ExceptionHandlers;

internal sealed class TimeOutExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<TimeOutExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not TimeoutException timeOutException)
        {
            return false;
        }

        logger.LogError(exception, "Unhandled time out exception occurred");

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = "A time out error has occurred",
                Detail = exception.Message
            }
        });
    }
}