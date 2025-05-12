using ProductCloud.SharedKernal.Common;

namespace SubscriptionService.Middleware
{
  public class ExceptionHandlingMiddleware : IMiddleware
  {
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
      _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
      try
      {
        await next(context);
      }
      catch (AppException ex)
      {
        context.Response.StatusCode = ex.Error.Code switch
        {
          "validation_error" => StatusCodes.Status400BadRequest,
          "not_found" => StatusCodes.Status404NotFound,
          _ => StatusCodes.Status500InternalServerError
        };

        await context.Response.WriteAsJsonAsync(new
        {
          error = ex.Error.Code,
          message = ex.Error.Message
        });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Unhandled exception occurred");

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new
        {
          error = "unexpected_error",
          message = "An unexpected error occurred"
        });
      }
    }
  }
}
