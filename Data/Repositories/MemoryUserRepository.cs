using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    public class MemoryUserRepository : IUserRepository
    {
        private readonly List<User> users = new();

        public void CreateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User is null");

            int id = GenerateId();
            user.Id = id;
            users.Add(user);
        }

        public User GetUser(Func<User, bool> predicate)
        {
            return users.FirstOrDefault(predicate);
        }

        public User GetUserById(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);

            if (user == null)
                throw new ArgumentNullException($"UserId {id} not found");

            return user;
        }

        public void UpdateLastLoginDate(int userId, DateTime dateTime)
        {
            var existingUser = GetUserById(userId);
            existingUser.LastLogin = dateTime;
        }

        private int GenerateId()
        {
            if (users.Count == 0)
                return 1;

            return users.Select(u => u.Id).Max() + 1;
        }
    }
}
