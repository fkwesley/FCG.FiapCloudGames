using FCG.Application.Interfaces;
using FCG.Application.Settings;
using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace FCG.Application.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly ILoggerRepository _loggerRepository;
        private readonly INewRelicLoggerRepository _newRelicLoggerRepository;
        private readonly bool _externalLoggerEnabled;

        public LoggerService(
            ILoggerRepository loggerRepository, 
            INewRelicLoggerRepository newRelicLoggerRepository, 
            IOptions<ExternalLoggerSettings> externalLogger)
        {
            _loggerRepository = loggerRepository ?? throw new ArgumentNullException(nameof(loggerRepository));
            _newRelicLoggerRepository = newRelicLoggerRepository ?? throw new ArgumentNullException(nameof(newRelicLoggerRepository));
            _externalLoggerEnabled = externalLogger?.Value?.Enabled ?? false;
        }

        public async Task LogTraceAsync(Trace trace)
        {
            if (trace == null || trace.LogId == null)
                throw new ArgumentNullException(nameof(trace), "Log entry or LogId cannot be null.");

            if (_externalLoggerEnabled)
                _newRelicLoggerRepository.SendLogAsync(trace);

            await _loggerRepository.LogTraceAsync(trace);
        }

        public async Task LogRequestAsync(RequestLog log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log), "Log cannot be null.");

            if (_externalLoggerEnabled && log.StatusCode != 0)
                _newRelicLoggerRepository.SendLogAsync(log);

            await _loggerRepository.LogRequestAsync(log);
        }

        public async Task UpdateRequestLogAsync(RequestLog log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log), "Log entry cannot be null.");

            if (_externalLoggerEnabled && log.StatusCode != 0)
                _newRelicLoggerRepository.SendLogAsync(log);

            await _loggerRepository.UpdateRequestLogAsync(log);
        }
    }
}
