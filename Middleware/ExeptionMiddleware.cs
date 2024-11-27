using System.Net;
using System.Text.Json;
using blog_api.Exeptions;

namespace blog_api.Middleware;

public class ExeptionMiddleware(RequestDelegate next, ILogger<ExeptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ProfileAlreadyExistsExeption ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (ProfileNotExistsExeption ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (PasswordNotExistsExeption ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.Unauthorized, ex.Message);
        }
        catch (CustomException customEx)
        {
            await HandleCustomExceptionAsync(context, customEx);
        }
        catch (Exception ex)
        {
            await HandleExceptionsAsync(context, ex);
        }
    }
    private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string errorMessage)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        var result = JsonSerializer.Serialize(new {errorMessage = errorMessage});
        return context.Response.WriteAsync(result);
    }
    
    private static Task HandleCustomExceptionAsync(HttpContext context, CustomException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception.StatusCode;

        return context.Response.WriteAsync(new
        {
            StatusCode = exception.StatusCode,
            Message = exception.Message
        }.ToString());
    }

    private static Task HandleExceptionsAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return context.Response.WriteAsync(new
        {
            StatusCode = StatusCodes.Status500InternalServerError,
            Message = "An unexpected error occurred"
        }.ToString());
    }
    
}
