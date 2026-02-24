namespace MasPatas.API.Middleware;

public class RequestContextMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestContextMiddleware> _logger;

    public RequestContextMiddleware(RequestDelegate next, ILogger<RequestContextMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var requestId = context.Request.Headers.TryGetValue("X-Request-Id", out var headerValue)
            ? headerValue.ToString()
            : Guid.NewGuid().ToString("N");

        context.Items["RequestId"] = requestId;
        _logger.LogInformation("{Method} {Path} RequestId={RequestId}", context.Request.Method, context.Request.Path, requestId);

        await _next(context);
    }
}
