using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SmartHome.Model.Errors;

namespace SmartHome.EventlogApi.ErrorHandling
{
    /// <summary>
    /// Обработчик ошибок
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IOptions<MvcNewtonsoftJsonOptions> _jsonOptions;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="next">Делегат</param>
        /// <param name="logger"></param>
        /// <param name="jsonOptions">JSON опции</param>
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IOptions<MvcNewtonsoftJsonOptions> jsonOptions)
        {
            _next = next;
            _logger = logger;
            _jsonOptions = jsonOptions;
        }
        public async Task Invoke(HttpContext context)
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode code;
            object errorObjeсt;

            switch (exception)
            {
                case EventlogApiException ex:
                {
                    errorObjeсt = ex.GetError();
                    code = HttpStatusCode.BadRequest;
                    break;
                }
                default:
                {
                    string absoluteURL = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
                    _logger.LogError(exception, $"Произошла непредвиденная ошибка: {absoluteURL}");
                    errorObjeсt = new ErrorModel { Error = exception.Message, ErrorCode = ErrorCode.UnspecifiedError };
                    code = HttpStatusCode.InternalServerError;
                    break;
                }
            }

            var result = JsonConvert.SerializeObject(errorObjeсt,Formatting.Indented, _jsonOptions.Value.SerializerSettings );
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
	}
}
