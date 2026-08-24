using Crumbs.API.Contracts.ResponseContracts;
using Crumbs.Domain.Services.Exceptions;
using System.Net;
using System.Text.Json;

namespace Crumbs.API.Middleware
{
    /// <summary>
    /// Vangt alle onbehandelde exceptions op en zet ze om naar een consistent JSON-foutformaat,
    /// zodat de frontend altijd dezelfde structuur kan verwachten en geen rauwe stack traces ziet.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, message) = exception switch
            {
                NotFoundException => (HttpStatusCode.NotFound, exception.Message),
                BadRequestException => (HttpStatusCode.BadRequest, exception.Message),
                ForbiddenException => (HttpStatusCode.Forbidden, exception.Message),
                ExternalServiceException => (HttpStatusCode.BadGateway, exception.Message),
                _ => (HttpStatusCode.InternalServerError, "Er is een onverwachte fout opgetreden.")
            };

            if (statusCode == HttpStatusCode.InternalServerError)
            {
                _logger.LogError(exception, "Onverwachte fout bij {Method} {Path}", context.Request.Method, context.Request.Path);
            }
            else
            {
                _logger.LogWarning("{ExceptionType} bij {Method} {Path}: {Message}",
                    exception.GetType().Name, context.Request.Method, context.Request.Path, exception.Message);
            }

            var response = new ErrorResponseContract
            {
                Message = message,
                Status = (int)statusCode
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
