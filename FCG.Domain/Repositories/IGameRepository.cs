using FCG.FiapCloudGames.Core.Entities;

namespace FCG.Domain.Repositories
{
    internal interface IGameRepository
    {
        IEnumerable<Game> GetAllGames();
        Game GetGameById(int id);
        Game AddGame(Game game);
        Game UpdateGame(Game game);
        bool DeleteGame(int id);
    }
}
