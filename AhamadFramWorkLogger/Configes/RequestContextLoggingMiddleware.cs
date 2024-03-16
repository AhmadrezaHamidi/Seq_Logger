using AhamadFramWorkLogger.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AhamadFramWorkLogger.Configes
{
    public class RequestContextLoggingMiddleware
    {
        private const string CorrelationIdHeaderName = "X-Correlation-Id";
        private readonly RequestDelegate _next;

        public RequestContextLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            string correlationId = Extenshions.GetCorrelationId(context, CorrelationIdHeaderName);

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                return _next.Invoke(context);
            }
        }

      

        ///   

    }
}
