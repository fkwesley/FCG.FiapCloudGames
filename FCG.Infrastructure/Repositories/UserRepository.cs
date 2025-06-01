using FCG.Domain.Repositories;
using FCG.FiapCloudGames.Core.Entities;
using FCG.Infrastructure.Context;

namespace FCG.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FiapCloudGamesDbContext _context;

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User? GetUserById(string userId)
        {
            return _context.Users.FirstOrDefault(g => g.UserId == userId);
        }

        public UserRepository(FiapCloudGamesDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public User AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User UpdateUser(User user)
        {
            var existingUser = GetUserById(user.UserId);

            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                existingUser.IsActive = user.IsActive;
                existingUser.IsAdmin = user.IsAdmin;
                existingUser.UpdatedAt = DateTime.UtcNow;
                
                _context.Users.Update(existingUser);
                _context.SaveChanges();

                return existingUser;
            }
            else
                throw new KeyNotFoundException($"User with ID {user.UserId} not found.");
        }

        public bool DeleteUser(string id)
        {
            var user = GetUserById(id);

            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return true;
            }
            else
                throw new KeyNotFoundException($"User with ID {id} not found.");
        }


    }
}
