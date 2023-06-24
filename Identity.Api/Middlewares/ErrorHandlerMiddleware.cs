using System.Net;
using Identity.Api.Models.CustomErrors;

namespace Identity.Api.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await httpContext.Response.WriteAsync(ex.Message);
            }
            catch (InvalidPasswordException ex)
            {
                _logger.LogError(ex, ex.Message);
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsync(ex.Message);
            }
            catch (TokenExpiredException ex)
            {
                _logger.LogError(ex, ex.Message);
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsync(ex.Message);
            }
            catch (AccountIsBlockedException ex)
            {
                _logger.LogError(ex, ex.Message);
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                var message = "Internal server error occurred";
                _logger.LogError(ex, message);
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await httpContext.Response.WriteAsync(message);
            }
        }
    }
}
