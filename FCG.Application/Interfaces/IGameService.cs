using FCG.FiapCloudGames.Core.Entities;

namespace FCG.Application.Interfaces
{
    public interface IGameService
    {
        IEnumerable<Game> GetAllGames();
        Game GetGameById(int id);
        Game AddGame(Game game);
        Game UpdateGame(Game game);
        bool DeleteGame(int id);
    }
}
