using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BikeRental.Api.Middleware;

/// <summary>
/// Глобальный обработчик исключений с логированием
/// </summary>
public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    /// <summary>
    /// Попытаться обработать исключение
    /// </summary>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // понятным сообщением сделать логи 
        LogExceptionWithSimpleMessage(httpContext, exception);
        
        var problemDetails = CreateProblemDetails(exception);

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problemDetails
            // 1. Пользователь кинул запрос например GET .../999
            // 2. Контроллер отсылает в NullReferenceException - тк bikeModelService.GetById(id) вернет null.
            // 3. ASP.NET Core ловит исключение
            // 4. Вызывает GlobalExceptionHandler.TryHandleAsync()
            // 5. логируем ошибку и создаем ProblemDetails, ProblemDetailsService генерирует JSON ответ

            // Возвращает true, если исключение было успешно обработано, false - если нужно пробросить дальше
            // TryWriteAsync возвращает false - клиент получает дефолтный ответ (500)
            // клиент не узнаёт что именно не так
            // но в консольке все выводится
        });
    }

    /// <summary>
    /// Логирование с короткими понятными сообщениями
    /// </summary>
    private void LogExceptionWithSimpleMessage(HttpContext httpContext, Exception exception)
    {
        var requestPath = httpContext.Request.Path;
        var method = httpContext.Request.Method;
        var exceptionType = exception.GetType().Name;

        // Основное понятное сообщение
        var message = exception switch
        {
            KeyNotFoundException keyEx => $"Resource not found: {keyEx.Message.Replace("not found", "")}",
            ArgumentException argEx => $"Invalid input: {argEx.Message}",
            InvalidOperationException opEx => $"Invalid operation: {opEx.Message}",
            UnauthorizedAccessException => "Access denied",
            _ => "Internal server error"
        };

        // Для 404 и 400 - Warning с кратким сообщением
        if (exception is KeyNotFoundException or ArgumentException or InvalidOperationException)
        {
            logger.LogWarning(
                "[{StatusCode}] {Method} {Path} - {Message}",
                GetStatusCode(exception),
                method,
                requestPath,
                message);
        }
        // Для остальных - Error с полным stack trace
        else
        {
            logger.LogError(
                exception,
                "[{StatusCode}] {Method} {Path} - {ExceptionType}: {Message}",
                GetStatusCode(exception),
                method,
                requestPath,
                exceptionType,
                message);
        }
    }

    /// <summary>
    /// Создание ProblemDetails
    /// </summary>
    private static ProblemDetails CreateProblemDetails(Exception exception)
    {
        var statusCode = GetStatusCode(exception);
        
        return new ProblemDetails
        {
            Title = GetTitle(exception),
            Detail = GetDetail(exception),
            Status = statusCode
        };
    }

    /// <summary>
    /// Получение статус кода
    /// </summary>
    private static int GetStatusCode(Exception exception) => exception switch
    {
        KeyNotFoundException => StatusCodes.Status404NotFound,
        ArgumentException => StatusCodes.Status400BadRequest,
        InvalidOperationException => StatusCodes.Status400BadRequest,
        UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
        _ => StatusCodes.Status500InternalServerError
    };

    /// <summary>
    /// Получение заголовка
    /// </summary>
    private static string GetTitle(Exception exception) => exception switch
    {
        KeyNotFoundException => "Resource not found",
        ArgumentException => "Bad request",
        InvalidOperationException => "Invalid operation",
        UnauthorizedAccessException => "Unauthorized",
        _ => "Internal server error"
    };

    /// <summary>
    /// Получение деталей
    /// </summary>
    private static string GetDetail(Exception exception)
    {
        // Для клиентских ошибок показываем сообщение исключения
        if (exception is KeyNotFoundException or ArgumentException or InvalidOperationException)
        {
            return exception.Message;
        }
        
        // Для серверных ошибок - общее сообщение
        return "An error occurred while processing your request. Please try again later.";
    }
}