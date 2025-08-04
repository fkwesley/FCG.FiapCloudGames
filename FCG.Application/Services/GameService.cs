using FCG.Application.DTO.Game;
using FCG.Application.Exceptions;
using FCG.Application.Interfaces;
using FCG.Application.Mappings;
using FCG.Domain.Entities;
using FCG.Domain.Enums;
using FCG.Domain.Repositories;
using FCG.FiapCloudGames.Core.Entities;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace FCG.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILoggerService _loggerService;
        private readonly IHttpContextAccessor _httpContext;
        public GameService(
                IGameRepository gameRepository, 
                ILoggerService loggerService,
                IHttpContextAccessor httpContext)
        {
            _gameRepository = gameRepository 
                ?? throw new ArgumentNullException(nameof(gameRepository));
            _loggerService = loggerService;
            _httpContext = httpContext;
        }

        public IEnumerable<GameResponse> GetAllGames()
        {
            var games = _gameRepository.GetAllGames();

            _loggerService.LogTraceAsync(new Trace
            {
                LogId = _httpContext.HttpContext?.Items["RequestId"] as Guid?,
                Timestamp = DateTime.UtcNow,
                Level = LogLevel.Info,
                Message = "Retrieved all games",
                StackTrace = null
            });

            return games.Select(game => game.ToResponse()).ToList();
        }

        public GameResponse GetGameById(int id)
        {
            var gameFound = _gameRepository.GetGameById(id);

            return gameFound.ToResponse();
        }

        public GameResponse AddGame(GameRequest game)
        {
            if (_gameRepository.GetAllGames().Any(g => g.Name == game.Name))
                throw new ValidationException(string.Format("Game {0} already exists.",game.Name));

            var gameEntity = game.ToEntity();
            var gameAdded = _gameRepository.AddGame(gameEntity);

            return gameAdded.ToResponse();
        }

        public GameResponse UpdateGame(GameRequest game)
        {
            if (_gameRepository.GetAllGames().Any(g => g.Name == game.Name && g.GameId != game.GameId))
                throw new ValidationException(string.Format("Game {0} already exists.", game.Name));

            var gameEntity = game.ToEntity();
            var gameUpdated = _gameRepository.UpdateGame(gameEntity);

            return gameUpdated.ToResponse();
        }

        public bool DeleteGame(int id)
        {
            return _gameRepository.DeleteGame(id);
        }

    }
}
