using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace BaseTest.API.Middlewave;

public class ExceptionHandlerMiddleWare : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            ProblemDetails details = new()
            {
                Detail = ex.Message,
                Status = (int) HttpStatusCode.InternalServerError,
                Title = "Server error",
                Type = "Server error",
            };
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(JsonSerializer.Serialize(details));
        }
    }
    
}
