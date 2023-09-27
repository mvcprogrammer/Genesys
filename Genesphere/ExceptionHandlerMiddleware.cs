using Newtonsoft.Json;

namespace Genesphere;

/// <summary>
/// Middleware for handling exceptions globally across the application.
/// It catches any exceptions thrown by the subsequent layers in the pipeline,
/// and returns a standardized error response to the client.
/// </summary>
public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlerMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware delegate in the pipeline.</param>
    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invokes the middleware to handle exceptions.
    /// If an exception is thrown by subsequent middleware, it will be caught,
    /// and a standardized error response will be returned to the client.
    /// </summary>
    /// <param name="context">The context for the current HTTP request.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles the exception by writing a standardized error response to the HTTP context.
    /// This method is called when an exception is caught by the <see cref="InvokeAsync"/> method.
    /// </summary>
    /// <param name="context">The context for the current HTTP request.</param>
    /// <param name="ex">The exception that was caught.</param>
    /// <returns>A Task that represents the asynchronous operation of writing the error response to the HTTP context.</returns>
    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
    
        var errorResponse = new 
        {
            context.Response.StatusCode,
            ex.Message
        };
    
        return context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
    }

}
