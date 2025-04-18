using Microsoft.AspNetCore.Diagnostics;

namespace ASPNET.Common.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
	private readonly RequestDelegate _next;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, IExceptionHandler customExceptionHandler)
    {
        try
        {
            await _next(httpContext);

            switch (httpContext.Response.StatusCode)
            {
                case StatusCodes.Status401Unauthorized:
                    await customExceptionHandler.TryHandleAsync(
                        httpContext,
                        new UnauthorizedAccessException("Unauthorized - Token missing or invalid"),
                        CancellationToken.None
                        );
                    break;
                case StatusCodes.Status403Forbidden:
                    await customExceptionHandler.TryHandleAsync(
                        httpContext,
                        new Exception("Forbidden - Access denied"),
                        CancellationToken.None
                        );
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            await customExceptionHandler.TryHandleAsync(httpContext, ex, CancellationToken.None );
        }
    }
}
