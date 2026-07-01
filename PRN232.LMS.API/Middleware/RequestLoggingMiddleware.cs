using System.Diagnostics;

namespace PRN232.LMS.API.Middleware
{
    /// <summary>
    /// Request logging middleware that logs HTTP request details including
    /// path, method, execution time, and response status code.
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestId = context.Request.Headers["X-Request-Id"].FirstOrDefault()
                            ?? Guid.NewGuid().ToString("N")[..8].ToUpper();

            _logger.LogInformation(
                "[{RequestId}] ⟹  {Method} {Path}{QueryString}",
                requestId,
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString);

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();

                var level = context.Response.StatusCode >= 500
                    ? LogLevel.Error
                    : context.Response.StatusCode >= 400
                        ? LogLevel.Warning
                        : LogLevel.Information;

                _logger.Log(level,
                    "[{RequestId}] ⟸  {Method} {Path} → {StatusCode} ({ElapsedMs}ms)",
                    requestId,
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
