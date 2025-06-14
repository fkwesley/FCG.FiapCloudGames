﻿using FCG.FiapCloudGames.Core.Entities;

namespace FCG.Domain.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User? GetUserById(string userId);
        User AddUser(User User);
        User UpdateUser(User User);
        bool DeleteUser(string userId);
    }
}
