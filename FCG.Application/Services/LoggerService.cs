using FCG.Application.Interfaces;
using FCG.Domain.Entities;
using FCG.Domain.Repositories;
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

        public LoggerService(ILoggerRepository loggerRepository)
        {
            _loggerRepository = loggerRepository 
                ?? throw new ArgumentNullException(nameof(loggerRepository));
        }

        public Task SaveLogAsync(LogEntry logEntry)
        {
            if (logEntry == null)
                throw new ArgumentNullException(nameof(logEntry), "Log entry cannot be null.");
            
            return _loggerRepository.SaveLogAsync(logEntry);
        }
    }
}
