using System.Text.Json.Serialization;

namespace FCG.Application.DTO.User
{
    public class UserRequest
    {
        public required string UserId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}
