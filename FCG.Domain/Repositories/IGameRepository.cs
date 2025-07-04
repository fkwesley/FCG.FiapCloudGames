﻿using FCG.FiapCloudGames.Core.Entities;

namespace FCG.Domain.Repositories
{
    public interface IGameRepository
    {
        IEnumerable<Game> GetAllGames();
        Game GetGameById(int id);
        Game AddGame(Game game);
        Game UpdateGame(Game game);
        bool DeleteGame(int id);
    }
}
