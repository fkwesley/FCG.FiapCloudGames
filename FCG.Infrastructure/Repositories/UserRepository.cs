﻿using FCG.Domain.Repositories;
using FCG.FiapCloudGames.Core.Entities;
using FCG.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

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
            // Procura por uma instância já rastreada desse usuário
            var trackedEntity = _context.ChangeTracker.Entries<User>().FirstOrDefault(e => e.Entity.UserId == user.UserId);

            // Desanexa a entidade rastreada para evitar conflito
            if (trackedEntity != null)
                trackedEntity.State = EntityState.Detached;

            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }

        public bool DeleteUser(string id)
        {
            var user = GetUserById(id);

            if (user != null)
            {
                user.IsActive = false;
                _context.Users.Update(user);
                _context.SaveChanges();
                return true;
            }
            else
                throw new KeyNotFoundException($"User with ID {id} not found.");
        }


    }
}
