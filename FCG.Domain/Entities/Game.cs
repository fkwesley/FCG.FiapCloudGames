namespace FCG.FiapCloudGames.Core.Entities
{
    public class Game
    {
        public int GameId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Genre { get; set; }
        public DateTime ReleaseDate { get; set; } = DateTime.Now;
        public int Rating { get; set; } = 0;
    }
}
