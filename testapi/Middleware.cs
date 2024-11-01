using System.Globalization;
using Mysqlx.Crud;
using System.Net;

namespace Middleware.Example;

public class RequestCultureMiddleware
{
    private readonly RequestDelegate _next;

    public RequestCultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {

        String? authorizationHeader = context.Request.Headers.Authorization;
        Console.WriteLine(authorizationHeader);

        if (!string.IsNullOrWhiteSpace(authorizationHeader))
        {
            String token = authorizationHeader;
            await _next(context);
        }
        else
        {
            Console.WriteLine("'Not'");
            return;
        }

        // Call the next delegate/middleware in the pipeline.
    }
}

public static class RequestCultureMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestCulture(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestCultureMiddleware>();
    }
}