using Reupload.Server.Dtos;
using Reupload.Server.Exceptions;

namespace Reupload.Server.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            ErrorResponseDto errorResponseDto = ExceptionHandler.Handle(ex);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)errorResponseDto.HttpStatusCode;

            await context.Response.WriteAsJsonAsync(errorResponseDto);
        }
    }
}