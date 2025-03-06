using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.ResponseModels;
using System.Data.Common;
using System.Net;

namespace Application.Middlewares
{
    public class ErrorHandlingMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleWare> _logger;

        public ErrorHandlingMiddleWare(RequestDelegate next, ILogger<ErrorHandlingMiddleWare> logger)
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
                _logger.LogError(ex, "An unexpected error occurred while processing the request.Path: {Path}, Method: {Method}, QueryString: {QueryString}",
                    context.Request.Path, context.Request.Method, context.Request.QueryString);

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var (message, statusCode) = exception switch
            {
                UnauthorizedAccessException e => (e.Message, HttpStatusCode.Unauthorized),
                ValidationException e => (e.Message, HttpStatusCode.UnprocessableEntity),
                KeyNotFoundException e => (e.Message, HttpStatusCode.NotFound),
                BadHttpRequestException e => (e.Message, HttpStatusCode.BadRequest),
                DbUpdateException e => (e.Message, HttpStatusCode.BadRequest),
                DbException e => ($"{e.Message}{(e.InnerException != null ? "\nInnerException: " + e.InnerException.Message : "")}", HttpStatusCode.InternalServerError),
                _ => (exception.Message, HttpStatusCode.InternalServerError)
            };

            var result = new Response<string>()
            {
                IsSucceeded = false,
                Message = message,
                StatusCode = statusCode
            };

            response.StatusCode = (int)statusCode;
            await response.WriteAsJsonAsync(result);
        }

    }
}
