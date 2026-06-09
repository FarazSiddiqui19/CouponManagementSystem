using CouponManagementSystem.DTO;
using Microsoft.AspNetCore.Diagnostics;

namespace CouponManagementSystem.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception");

        httpContext.Response.StatusCode = 500;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(
            new ErrorResponse { message = "An internal error occurred" },
            cancellationToken);

        return true;
    }
}
