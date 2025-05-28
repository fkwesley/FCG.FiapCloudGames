using FCG.Application.DTO.User;
using FCG.FiapCloudGames.Core.Entities;

namespace FCG.Application.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserResponse> GetAllUsers();
        UserResponse GetUserById(int id);
        UserResponse AddUser(UserRequest user);
        UserResponse UpdateUser(UserRequest user);
        bool DeleteUser(int id);
    }
}