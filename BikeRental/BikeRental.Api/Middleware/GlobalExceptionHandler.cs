using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BikeRental.Api.Middleware;

/// <summary>
/// Глобальный обработчик исключений
/// </summary>
/// <param name="problemDetailsService"></param>
public sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler // - интерфейс в .NET 8 для обработки исключений
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
            HttpContext = httpContext, // получение информации о запросе
            Exception = exception, // для логирования и диагностики
            ProblemDetails = new ProblemDetails // базовая информация об ошибке
            {
                Title = "Internal Server Error",
                Detail = "An error occurred while processing your request. Please try again"
            }
            // TryWriteAsync() пишет ответ в поток HTTP
            // 1. Пользователь кинул запрос например GET .../999
            // 2. Контроллер отсылает в NullReferenceException - тк bikeModelService.GetById(id) вернет null.
            // 3. ASP.NET Core ловит исключение
            // 4. Вызывает GlobalExceptionHandler.TryHandleAsync()
            // 5. ProblemDetailsService генерирует JSON ответ
        });
    }
}
