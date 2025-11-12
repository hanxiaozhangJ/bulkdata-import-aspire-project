using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkDataImport.Api;

public sealed class DefaultExceptionHandler(ILogger<DefaultExceptionHandler> logger, IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unexpected exception");
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = {
                Title = "Unexpected error",
                Detail = "An unexpected error occurred."
            }
        });
    }
}

