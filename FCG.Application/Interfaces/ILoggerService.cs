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
        Task LogTraceAsync(Trace trace);

        Task LogRequestAsync(RequestLog logEntry);
        Task UpdateRequestLogAsync(RequestLog logEntry);

    }
}
