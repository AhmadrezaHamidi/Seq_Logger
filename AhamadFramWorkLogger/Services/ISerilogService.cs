using AhamadFramWorkLogger.Models;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace AhamadFramWorkLogger.Services
{
    public interface ISerilogService
    {
        void CustomLog(LogLevel logLevel, LogType logType, string TraceId, string source, string serviceName, [CallerLineNumber] int line = 0, string? userId = "", string? description = "", object? data = null,
            string? exception = "");
    }
    public sealed class SerilogService : ISerilogService
    {
        public void CustomLog(LogLevel logLevel, LogType logType, string TraceId, string source, string serviceName, [CallerLineNumber] int line = 0, string? userId = "", string? description = "", object? data = null, string? exception = "")
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };
            EventLogDto eventLog = new EventLogDto
            {
                Source = source,
                UserId = userId,
                ServiceName = serviceName,
                logLevel = logLevel,
                Line = line,
                Data = JsonSerializer.Serialize(data, options),
                Description = description,
                Exception = exception
            };
            Info(eventLog);
        }

        private static void Info(EventLogDto infoToLog)
        {
            Log.Information(
                    "{CreatedAt} {ServiceName} {LogLevel} " +
                  "{UserId} {Line} {Description} " +
             "{Source} {Data} {Exception} {LogType} {TraceId}",
                infoToLog.CreatedAt, infoToLog.ServiceName, infoToLog.logLevel,
               infoToLog.UserId, infoToLog.Line, infoToLog.Description,
            infoToLog.Source, infoToLog.Data, infoToLog.Exception,
                 infoToLog.LogType, infoToLog.TraceId);

        }


    }
}
