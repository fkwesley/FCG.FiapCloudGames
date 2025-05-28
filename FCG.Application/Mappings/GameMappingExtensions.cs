using FCG.Application.DTO.Game;
using FCG.FiapCloudGames.Core.Entities;

namespace FCG.Application.Mappings
{
    public static class GameMappingExtensions
    {
        /// <summary>   
        /// Maps a GameRequest to a Game entity.
        public static Game ToEntity(this GameRequest request)
        {
            return new Game
            {
                GameId = request.GameId,
                Name = request.Name,
                Description = request.Description,
                Genre = request.Genre,
                ReleaseDate = request.ReleaseDate,
                Rating = request.Rating
            };
        }

        /// <summary>
        /// maps a Game entity to a GameResponse.
        public static GameResponse ToResponse(this Game entity)
        {
            return new GameResponse
            {
                GameId = entity.GameId,
                Name = entity.Name,
                Description = entity.Description,
                Genre = entity.Genre,
                ReleaseDate = entity.ReleaseDate,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Rating = entity.Rating
            };
        }
    }
}
