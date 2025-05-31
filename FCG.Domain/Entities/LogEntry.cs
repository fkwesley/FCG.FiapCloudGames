namespace FCG.Domain.Entities
{
    public class LogEntry
    {
        public Guid LogId { get; set; } = Guid.NewGuid();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Level { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
