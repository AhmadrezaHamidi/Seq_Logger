using AhamadFramWorkLogger.Models;
using AhamadFramWorkLogger.Services;
using AhamadFramWorkLogger.Tools;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhamadFramWorkLogger.Configes
{
    public class Result
    {
        public Result() { }
        public bool IsSucccess { get; set; }
        public object Datas { get; set; }
        public List<string> Error { get; set; }
    }

    internal sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class
        where TResponse : Result
    {
        private readonly ISerilogService _logger;
        private string source = "Logger.Services";
        private string serviceName = "RequestLogging";
        private readonly IHttpContextAccessor _httpContextAccessor;


        public RequestLoggingPipelineBehavior(ISerilogService logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            string requestName = typeof(TRequest).Name;
            var userId = "System";

            var context = _httpContextAccessor.HttpContext;
            var correlationId = Extenshions.GetCorrelationId(context);
            _logger.CustomLog(
                   Microsoft.Extensions.Logging.LogLevel.Trace,
                   LogType.Input,
                   correlationId,
                   source,
                   serviceName,
                   line: 54,
                   userId: userId,
                   description: $"Starting request {requestName}",
                   data: request, // Assuming you want to log the request details
                   exception: null);

            TResponse result = await next();

            if (result.IsSucccess)
            {
                //_logger.CustomLog(
                //    "Completed request {RequestName}", requestName);

                _logger.CustomLog(
                      Microsoft.Extensions.Logging.LogLevel.Information,
                      LogType.OutPut,
                    correlationId,
                 source,
                 serviceName,
                 line: 73, // Adjust line number as appropriate
                 userId: userId,
                description: $"Completed request {requestName} successfully.",
                    data: result, // Assuming you want to log the result
                 exception: null);

            }
            else
            {
                using (LogContext.PushProperty("Error", result, true))
                {
                    _logger.CustomLog(
                 Microsoft.Extensions.Logging.LogLevel.Error,
           LogType.Error,
           correlationId,
           source,
           serviceName,
           line: 92, 
           userId: userId,
           description: $"Completed request {requestName} with error: {result.Error}",
           data: result, // You might want to log the result even in case of error
           exception: string.Join(',',result.Error));
                }
            }

            return result;
        }
    }
}
