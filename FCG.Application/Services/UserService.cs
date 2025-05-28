using FCG.Application.DTO.User;
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

        public UserResponse GetUserById(int id)
        {
            var userFound = _userRepository.GetUserById(id);

            return userFound.ToResponse();
        }

        public UserResponse AddUser(UserRequest user)
        {
            var userEntity = user.ToEntity();
            var userAdded = _userRepository.AddUser(userEntity);
                
            return userAdded.ToResponse();
        }

        public UserResponse UpdateUser(UserRequest user)
        {
            var userEntity = user.ToEntity();
            var userUpdated = _userRepository.UpdateUser(userEntity);

            return userUpdated.ToResponse();
        }

        public bool DeleteUser(int id)
        {
            return _userRepository.DeleteUser(id);
        }

    }
}
