using FCG.Application.Interfaces;
using FCG.Domain.Repositories;
using FCG.FiapCloudGames.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public IEnumerable<Game> GetAllGames()
        {
            return _gameRepository.GetAllGames().ToList();
        }

        public Game GetGameById(int id)
        {
            return _gameRepository.GetGameById(id);
        }

        public Game AddGame(Game game)
        {
            return _gameRepository.AddGame(game);
        }

        public Game UpdateGame(Game game)
        {
            return _gameRepository.UpdateGame(game);
        }
        public bool DeleteGame(int id)
        {
            return _gameRepository.DeleteGame(id);
        }

    }
}
