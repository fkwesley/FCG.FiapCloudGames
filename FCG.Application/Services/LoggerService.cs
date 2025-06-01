using FCG.Application.Interfaces;
using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly ILoggerRepository _loggerRepository;
        private readonly IHttpContextAccessor _httpContext;

        public LoggerService(ILoggerRepository loggerRepository, IHttpContextAccessor httpContext)
        {
            _loggerRepository = loggerRepository
                ?? throw new ArgumentNullException(nameof(loggerRepository));
            _httpContext = httpContext;
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


            return _loggerRepository.LogTraceAsync(trace);
        }

        public Task LogRequestAsync(RequestLog log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log), "Log cannot be null.");

            return _loggerRepository.LogRequestAsync(log);
        }

        public Task UpdateRequestLogAsync(RequestLog log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log), "Log entry cannot be null.");

            return _loggerRepository.UpdateRequestLogAsync(log);
        }
    }
}
