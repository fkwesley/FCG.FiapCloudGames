﻿using FCG.Application.DTO.Game;
using FCG.Application.Exceptions;
using FCG.Application.Interfaces;
using FCG.Application.Mappings;
using FCG.Domain.Entities;
using FCG.Domain.Enums;
using FCG.Domain.Repositories;
using FCG.FiapCloudGames.Core.Entities;

namespace FCG.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILoggerService _loggerService;
        public GameService(IGameRepository gameRepository, ILoggerService loggerService)
        {
            _gameRepository = gameRepository 
                ?? throw new ArgumentNullException(nameof(gameRepository));
            _loggerService = loggerService;
        }

        public IEnumerable<GameResponse> GetAllGames()
        {
            var games = _gameRepository.GetAllGames();

            _loggerService.LogTraceAsync(new Trace
            {
                Timestamp = DateTime.UtcNow,
                Level = LogLevel.Info,
                Message = "Retrieved all games",
                StackTrace = null
            });

            return games.Select(game => game.ToResponse()).ToList();
        }

        public GameResponse GetGameById(int gameId)
        {
            var gameFound = _gameRepository.GetGameById(gameId);

            if (gameFound == null)
                throw new KeyNotFoundException($"Game with ID {gameFound} not found.");

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
