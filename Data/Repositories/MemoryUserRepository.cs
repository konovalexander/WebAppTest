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

        public MemoryUserRepository()
        {
            users.Add(new User
            {
                Id = 1,
                FIO = "Коновалов Александр Владимирович",
                Phone = "79507437731",
                Email = "konovalexander@rambler.ru",
                Password = "123",
                LastLogin = new DateTime()
            });
        }

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

        public void UpdateUser(User user)
        {
            var existingUser = GetUserById(user.Id);
            existingUser.LastLogin = DateTime.UtcNow;            
        }

        private int GenerateId()
        {
            if (users.Count == 0)
                return 1;

            return users.Select(u => u.Id).Max() + 1;
        }
    }
}
