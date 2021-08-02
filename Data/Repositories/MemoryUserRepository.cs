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

        public bool CheckUserExists(string phone, string email)
        {
            return users.Any(u => u.Phone == phone || u.Email == email);
        }

        public void CreateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User is null");

            int id = GenerateId();
            user.Id = id;
            users.Add(user);
        }

        public User GetUserByPhone(string phone)
        {
            return users.FirstOrDefault(u => u.Phone == phone);
        }

        public void UpdateLastLoginDate(string phone, DateTime dateTime)
        {
            var existingUser = GetUserByPhone(phone);
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
