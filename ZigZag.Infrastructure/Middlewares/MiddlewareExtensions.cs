using Microsoft.AspNetCore.Builder;

namespace ZigZag.Infrastructure.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static void ConfigureMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}
