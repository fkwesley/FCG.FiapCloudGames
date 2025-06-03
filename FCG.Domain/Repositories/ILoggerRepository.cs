using FCG.Domain.Entities;

namespace FCG.Domain.Repositories
{
    public interface ILoggerRepository
    {
        public Task LogTraceAsync(Trace log);
        public Task LogRequestAsync(RequestLog log);

        public Task UpdateRequestLogAsync(RequestLog log);
    }
}
