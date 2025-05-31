using FCG.Domain.Entities;

namespace FCG.Domain.Repositories
{
    public interface ILoggerRepository
    {
        public Task SaveLogAsync(LogEntry logEntry);
    }
}
