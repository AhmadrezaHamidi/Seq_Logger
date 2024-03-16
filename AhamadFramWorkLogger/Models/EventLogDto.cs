using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhamadFramWorkLogger.Models
{
    public sealed class EventLogDto
    {
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public Microsoft.Extensions.Logging.LogLevel logLevel { get; set; }
        public required string Source { get; set; }
        public string? ServiceName { get; set; }
        public int Line { get; set; }
        public string? UserId { get; set; }
        public string? Description { get; set; }
        public object? Data { get; set; }
        public string? Exception { get; set; }
        public LogType LogType { get; set; }
        public string TraceId { get; set; }
    }
}
