using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhamadFramWorkLogger.Tools
{
    internal static class Extenshions
    {
        public  static string GetCorrelationId(HttpContext context,string? CorrelationIdHeaderName = "X-Correlation-Id")
        {
            context.Request.Headers.TryGetValue(
                CorrelationIdHeaderName, out StringValues correlationId);

            return correlationId.FirstOrDefault() ?? context.TraceIdentifier;
        }
       
    }
}
