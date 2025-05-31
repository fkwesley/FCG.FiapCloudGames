using FCG.Application.DTO.User;
using FCG.Application.Exceptions;
using FCG.Application.Interfaces;
using FCG.Application.Mappings;
using FCG.Domain.Repositories;
using FCG.FiapCloudGames.Core.Entities;

namespace FCG.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository
                ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public IEnumerable<UserResponse> GetAllUsers()
        {
            var users = _userRepository.GetAllUsers();

            return users.Select(user => user.ToResponse()).ToList();
        }

        public UserResponse GetUserById(string userId)
        {
            var userFound = _userRepository.GetUserById(userId);

            return userFound.ToResponse();
        }

        public UserResponse AddUser(UserRequest user)
        {
            var activeUsers = _userRepository.GetAllUsers().Where(u => u.IsActive);

            if (activeUsers.Any(u => u.UserId == user.UserId.ToUpper()))
                throw new ValidationException("UserId already exists. Try another one.");

            if (activeUsers.Any(u => u.Email == user.Email.ToLower()))
                throw new ValidationException("E-mail already used by another active user. Try another one.");

            var userEntity = user.ToEntity();
            var userAdded = _userRepository.AddUser(userEntity);
            
            return userAdded.ToResponse();
        }

        public UserResponse UpdateUser(UserRequest user)
        {
            var activeUsers = _userRepository.GetAllUsers().Where(u => u.IsActive && u.UserId != user.UserId);

            if (activeUsers.Any(u => u.Email == user.Email.ToLower()))
                throw new ValidationException("E-mail already used by another active user. Try another one.");

            var userEntity = user.ToEntity();
            var userUpdated = _userRepository.UpdateUser(userEntity);

            return userUpdated.ToResponse();
        }

        public bool DeleteUser(string userId)
        {
            return _userRepository.DeleteUser(userId);
        }

        public User? ValidateCredentials(string userId, string password)
        {
            var userFound = _userRepository.GetUserById(userId.ToUpper());

            if (userFound != null && userFound.Password == password)
                return userFound;
            else
                throw new UnauthorizedAccessException("User or password invalid.");
        }

    }
}
