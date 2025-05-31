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
        public async Task SaveLogAsync(LogEntry logEntry)
        {
            // Adiciona a entidade LogEntry no contexto
            await _context.LogEntries.AddAsync(logEntry);

            // Persiste as alterações no banco de dados
            await _context.SaveChangesAsync();
        }
    }
}
