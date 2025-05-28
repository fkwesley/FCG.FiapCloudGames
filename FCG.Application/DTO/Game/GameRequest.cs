using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FCG.Application.DTO.Game
{
    public class GameRequest
    {
        [JsonIgnore]
        public int GameId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Genre { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public int? Rating { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }
    }
}
