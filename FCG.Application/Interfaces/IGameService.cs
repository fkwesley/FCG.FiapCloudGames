using FCG.Application.DTO.Game;
using FCG.FiapCloudGames.Core.Entities;

namespace FCG.Application.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<GameResponse>> GetAllGamesAsync();
        GameResponse GetGameById(int id);
        GameResponse AddGame(GameRequest game);
        GameResponse UpdateGame(GameRequest game);
        bool DeleteGame(int id);
    }
}
