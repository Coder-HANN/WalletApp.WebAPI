using System;

namespace WalletApp.Logging.Models
{
    public class ApplicationLog
    {
        public int Id { get; set; }
        public string Level { get; set; } = "Info"; // Info, Error, Warning
        public string Message { get; set; }
        public string Exception { get; set; }
        public string RequestPath { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public int? StatusCode { get; set; }
        public string IpAddress { get; set; }
        public string MachineName { get; set; }
        public DateTime RequestTime { get; set; }
        public long DurationMs { get; set; }
    }
}
