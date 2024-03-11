using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using System.Threading;

public static class LoggerFactory
{
    private static readonly ILoggerFactory _internalLoggerFactory = new LoggerFactory();

    public static ILoggerFactory GetInstance()
    {
        return _internalLoggerFactory;
    }

    public static ILogger CreateLogger<T>()
    {
        return _internalLoggerFactory.CreateLogger<T>();
    }
}

public interface ILoggingStrategy
{
    void Log(string message);
}

public class ConsoleLoggingStrategy : ILoggingStrategy
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}

public class FileLoggingStrategy : ILoggingStrategy
{
    public void Log(string message)
    {
        // Logic to log to a file
    }
}

public static class LoggingExtensions
{
    public static void Log<T>(this ILogger logger, T logEntry)
    {
        // Serialize logEntry if necessary and log
    }
}

public class RequestContextLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestContextLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        string correlationId = context.Request.Headers.TryGetValue("X-Correlation-Id", out var values)
            ? values.FirstOrDefault()
            : context.TraceIdentifier;

        if (correlationId == null)
        {
            return; // Early return if correlation ID not found
        }

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await _next(context);
        }
    }
}

public interface ISerilogService
{
    void CustomLog(LogLevel logLevel, string source, string serviceName, int line = 0, string? userId = "", string? description = "", object? data = null, string? exception = "");
}

public class SerilogService : ISerilogService
{
    private readonly ILogger _logger;

    public SerilogService(ILogger<SerilogService> logger)
    {
        _logger = logger;
    }

    public void CustomLog(LogLevel logLevel, string source, string serviceName, int line = 0, string? userId = "", string? description = "", object? data = null, string? exception = "")
    {
        var logEvent = new LogEventInfo
        {
            Level = logLevel,
            Message = description ?? string.Empty,
            Properties = {
                ["Source"] = source,
                ["ServiceName"] = serviceName,
                ["Line"] = line,
                ["UserId"] = userId ?? string.Empty,
                ["Data"] = data,
                ["Exception"] = exception ?? string.Empty,
                ["TimeStamp"] = DateTime.UtcNow
            }
        };

        // Log based on LogLevel using Serilog
        _logger.Log(logEvent.Level, "{@LogEvent}", logEvent);
    }
}

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestContextLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestContextLoggingMiddleware>();
    }
}

public class RequestLoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> _logger;

    public RequestLoggingPipelineBehavior(ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _logger.LogInformation("Handling {RequestName}", typeof(TRequest).Name);

        try
        {
            var response = await next();
            _logger.LogInformation("Handled {RequestName}", typeof(TRequest).Name);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling {RequestName}", typeof(TRequest).Name);
            throw;
        }
    }
}
