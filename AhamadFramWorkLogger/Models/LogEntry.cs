using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AhamadFramWorkLogger.Models
{
    public class LogEntry
    {
        public DateTime CreatedAt { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Source { get; set; }
        public string ServiceName { get; set; }
        public int Line { get; set; } = 0;
        public string? UserId { get; set; } = "";
        public string? Description { get; set; } = "";
        public string? Data { get; set; } = null;
        public string? Exception { get; set; } = "";
        public LogType LogType { get; set; }
        public string TraceId { get; set; }
        public LogEntry(string traceId, LogLevel logLevel, LogType logType, string source, string serviceName, [CallerLineNumber] int line = 0,
            string? userId = "", string? description = "", object? data = null, string? exception = "")
        {
            TraceId = traceId;
            LogType = logType;
            LogLevel = logLevel;
            Source = source;
            ServiceName = serviceName;
            Line = line;
            UserId = userId;
            Description = description;
            Data = data.ToString();
            Exception = exception;
        }

        public override string ToString()
        {
            return $"LogLevel: {LogLevel}, Source: {Source}, ServiceName: {ServiceName}, Line: {Line}, UserId: {UserId}, Description: {Description}, Data: {Data}, Exception: {Exception}";
        }
    }
    public enum LogType
    {
        Trace,
        Input,
        OutPut,
        Error
    }
}
