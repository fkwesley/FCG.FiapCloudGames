using FCG.Application.DTO.User;
using FCG.FiapCloudGames.Core.Entities;

namespace FCG.Application.Mappings
{
    public static class UserMappingExtensions
    {
        /// <summary>   
        /// Maps a UserRequest to a User entity.
        public static User ToEntity(this UserRequest request)
        {
            return new User
            {
                UserId = request.UserId.ToUpper(), 
                Name = request.Name.ToUpper(),         
                Email = request.Email.ToLower(),       
                Password = request.Password.ToUpper(), 
                IsActive = request.IsActive,   
                IsAdmin = request.IsAdmin              
            };
        }

        /// <summary>
        /// maps a User entity to a UserResponse.
        public static UserResponse ToResponse(this User entity)
        {
            return new UserResponse
            {
                UserId = entity.UserId,
                Name = entity.Name,
                Email = entity.Email,
                Password = entity.Password,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                IsAdmin = entity.IsAdmin
            };
        }
    }
}
