using FCG.Application.DTO.Game;
using FCG.Application.Interfaces;
using FCG.Application.Mappings;
using FCG.Domain.Repositories;

namespace FCG.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository 
                ?? throw new ArgumentNullException(nameof(gameRepository));
        }

        public IEnumerable<GameResponse> GetAllGames()
        {
            var games = _gameRepository.GetAllGames();

            return games.Select(game => game.ToResponse()).ToList();
        }

        public GameResponse GetGameById(int id)
        {
            var gameFound = _gameRepository.GetGameById(id);

            return gameFound.ToResponse();
        }

        public GameResponse AddGame(GameRequest game)
        {
            var gameEntity = game.ToEntity();
            var gameAdded = _gameRepository.AddGame(gameEntity);

            return gameAdded.ToResponse();
        }

        public GameResponse UpdateGame(GameRequest game)
        {
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
