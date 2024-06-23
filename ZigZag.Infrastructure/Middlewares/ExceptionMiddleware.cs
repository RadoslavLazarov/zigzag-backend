using System.Net;
using System.Security;
using ZigZag.Domain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ZigZag.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace ZigZag.Infrastructure.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly IWebHostEnvironment env;

        public ExceptionMiddleware(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var jsonSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

            var error = new ErrorModel()
            {
                Title = exception is ValidationException ?
                    (exception as ValidationException).Title : exception is NotAcceptableException ?
                        (exception as NotAcceptableException).Title : string.Empty,
                Message = exception.Message,
                StackTrace = env.EnvironmentName == "Development" ? exception.StackTrace : null,
                ExceptionType = env.EnvironmentName == "Development" ? exception.GetType().FullName : null
            };

            var result = JsonConvert.SerializeObject(error, jsonSettings);

            context.Response.Clear();
            context.Response.StatusCode = GetHttpStatusCode(exception);
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(result);
        }

        private static int GetHttpStatusCode(Exception exception) => exception switch
        {
            ValidationException
                => (int)HttpStatusCode.BadRequest,

            SecurityException _ or
            SecurityTokenExpiredException _ or
            NotAuthorizedException _
                => (int)HttpStatusCode.Unauthorized,

            InsufficientAccessException _
                => (int)HttpStatusCode.Forbidden,

            NotFoundException _
                => (int)HttpStatusCode.NotFound,

            MethodAccessException _
                => (int)HttpStatusCode.MethodNotAllowed,

            NotAcceptableException _
                => (int)HttpStatusCode.NotAcceptable,

            _ => (int)HttpStatusCode.InternalServerError,
        };
    }
}
