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
        private readonly IHttpContextAccessor _httpContext;
        private readonly bool _externalLoggerEnabled;

        public LoggerService(
            ILoggerRepository loggerRepository, 
            INewRelicLoggerRepository newRelicLoggerRepository, 
            IHttpContextAccessor httpContext,
            IOptions<ExternalLoggerSettings> externalLogger)
        {
            _loggerRepository = loggerRepository ?? throw new ArgumentNullException(nameof(loggerRepository));
            _newRelicLoggerRepository = newRelicLoggerRepository ?? throw new ArgumentNullException(nameof(newRelicLoggerRepository));
            _httpContext = httpContext;
            _externalLoggerEnabled = externalLogger?.Value?.Enabled ?? false;
        }

        public Task LogTraceAsync(Trace trace)
        {
            if (trace == null)
                throw new ArgumentNullException(nameof(trace), "Log entry cannot be null.");

            if (trace.LogId == null)
            {
                // If LogId is not set, try to retrieve it from the current HTTP context
                var context = _httpContext.HttpContext;

                if (context != null && context.Items.TryGetValue("RequestId", out var requestIdObj) && requestIdObj is Guid requestId)
                        trace.LogId = requestId;
            }

            if (_externalLoggerEnabled)
                _newRelicLoggerRepository.SendLogAsync(trace);

            return _loggerRepository.LogTraceAsync(trace);
        }

        public Task LogRequestAsync(RequestLog log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log), "Log cannot be null.");

            if (_externalLoggerEnabled && log.StatusCode != 0)
                _newRelicLoggerRepository.SendLogAsync(log);

            return _loggerRepository.LogRequestAsync(log);
        }

        public Task UpdateRequestLogAsync(RequestLog log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log), "Log entry cannot be null.");

            if (_externalLoggerEnabled && log.StatusCode != 0)
                _newRelicLoggerRepository.SendLogAsync(log);

            return _loggerRepository.UpdateRequestLogAsync(log);
        }
    }
}
