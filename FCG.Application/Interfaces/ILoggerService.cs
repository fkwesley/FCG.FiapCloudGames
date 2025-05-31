using FCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Interfaces
{
    public interface ILoggerService
    {
        public Task SaveLogAsync(LogEntry logEntry);
    }
}
