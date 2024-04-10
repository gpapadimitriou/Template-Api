using Application.Common.Exceptions;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace FarmaSenseAuth.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            using MemoryStream injectedRequestStream = new();


            try
            {
                var requestLog = $"REQUEST HttpMethod: {context.Request.Method}, Path: {context.Request.Path}";
                if (context.Request.Method == "GET")
                {
                    if (context.Request.QueryString.HasValue)
                        requestLog += $", QueryParameters : {context.Request.QueryString.Value}";
                }
                else
                {
                    using var bodyReader = new StreamReader(context.Request.Body);
                    var bodyAsText = await bodyReader.ReadToEndAsync();
                    if (string.IsNullOrWhiteSpace(bodyAsText) == false)
                    {
                        requestLog += $", Body : {bodyAsText}";
                    }

                    var bytesToWrite = Encoding.UTF8.GetBytes(bodyAsText);
                    injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
                    injectedRequestStream.Seek(0, SeekOrigin.Begin);
                    context.Request.Body = injectedRequestStream;
                }
                _logger.LogInformation(requestLog);

                await _next(context);
            }
            catch (Exception ex)
            {
                await ConvertException(context, ex);
            }
            finally
            {
                injectedRequestStream.Dispose();
            }
        }

        private Task ConvertException(HttpContext context, Exception exception)
        {
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";
            var result = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(validationException.Errors);
                    foreach (var error in validationException.Errors)
                    {
                        foreach (var errorValue in error.Value)
                        {
                            _logger.LogError($"Validation exception details key: {error.Key} value: {errorValue}");
                        }
                    }
                    break;
                case BadRequestException badRequestException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = badRequestException.Message;
                    _logger.LogError($"Bad Request exception  details : {result}");

                    break;
                //case NotFoundException notFoundException:
                //    httpStatusCode = HttpStatusCode.NotFound;
                //    break;
                case Exception ex:
                    _logger.LogError($"General exception message: {ex?.InnerException?.Message}");
                    _logger.LogError($"General exception complete : {ex}");
                    //  httpStatusCode = HttpStatusCode.BadRequest;
                    break;
            }

            context.Response.StatusCode = (int)httpStatusCode;

            if (result == string.Empty)
            {
                result = JsonConvert.SerializeObject(new { error = exception.Message });
            }

            return context.Response.WriteAsync(result);
        }

    }
}
