namespace FCG.FiapCloudGames.Core.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }         
        public required string Password { get; set; }
        public bool IsActive { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}
