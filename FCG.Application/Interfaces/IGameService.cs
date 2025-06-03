using FCG.Application.DTO.Game;
using FCG.FiapCloudGames.Core.Entities;

namespace FCG.Application.Interfaces
{
    public interface IGameService
    {
        IEnumerable<GameResponse> GetAllGames();
        GameResponse GetGameById(int id);
        GameResponse AddGame(GameRequest game);
        GameResponse UpdateGame(GameRequest game);
        bool DeleteGame(int id);
    }
}
