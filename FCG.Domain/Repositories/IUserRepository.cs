using FCG.FiapCloudGames.Core.Entities;

namespace FCG.Domain.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        User AddUser(User User);
        User UpdateUser(User User);
        bool DeleteUser(int id);
    }
}
