using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BikeRental.Api.Middleware;

/// <summary>
/// Глобальный обработчик исключений
/// </summary>
public sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{   
    /// <summary>
    /// Попытаться обработать исключение
    /// </summary>
    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken) 
    {
        return problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = "An error occurred while processing your request. Please try again"
            }
            
        });
    }
}
