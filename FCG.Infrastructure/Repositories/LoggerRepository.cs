using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Infrastructure.Context;
using System;

namespace FCG.Infrastructure.Repositories
{
    public class LoggerRepository : ILoggerRepository
    {
        private readonly FiapCloudGamesDbContext _context; // Seu DbContext para acesso ao banco

        // Injeta o DbContext via construtor
        public LoggerRepository(FiapCloudGamesDbContext context)
        {
            _context = context;
        }

        // Método para salvar o log no banco de forma assíncrona
        public async Task LogTraceAsync(Trace log)
        {
            // Adiciona a entidade LogEntry no contexto
            await _context.Traces.AddAsync(log);

            // Persiste as alterações no banco de dados
            await _context.SaveChangesAsync();
        }

        public Task LogRequestAsync(RequestLog log)
        {
            // Adiciona a entidade RequestLog no contexto
            _context.RequestLogs.Add(log);

            // Persiste as alterações no banco de dados
            return _context.SaveChangesAsync();
        }

        public Task UpdateRequestLogAsync(RequestLog log)
        {
            // Atualiza a entidade RequestLog no contexto
            _context.RequestLogs.Update(log);

            // Persiste as alterações no banco de dados
            return _context.SaveChangesAsync();
        }
    }
}
