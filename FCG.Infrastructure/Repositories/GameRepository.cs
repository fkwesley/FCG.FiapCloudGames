using FCG.Domain.Repositories;
using FCG.FiapCloudGames.Core.Entities;
using FCG.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly FiapCloudGamesDbContext _context;

        public GameRepository(FiapCloudGamesDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Game> GetAllGames()
        {
           return _context.Game.ToList();
        }

        public Game GetGameById(int id)
        {
            return _context.Game.FirstOrDefault(g => g.GameId == id) 
                ?? throw new KeyNotFoundException($"Game with ID {id} not found.");
        }

        public Game AddGame(Game game)
        {
            _context.Game.Add(game);
            _context.SaveChanges();
            return game;
        }

        public Game UpdateGame(Game game)
        {
            var existingGame = GetGameById(game.GameId);

            if (existingGame != null) {
                existingGame.Name = game.Name;
                existingGame.Description = game.Description;
                existingGame.Genre = game.Genre;
                existingGame.ReleaseDate = game.ReleaseDate;
                existingGame.UpdatedAt = DateTime.Now; 
                existingGame.Rating = game.Rating;
                
                _context.Game.Update(existingGame);
                _context.SaveChanges();
            }

            return existingGame;
        }

        public bool DeleteGame(int id)
        {
            var game = GetGameById(id);

            if (game != null)
            {
                _context.Game.Remove(game);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
