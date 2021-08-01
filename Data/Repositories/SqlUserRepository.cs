using Data.Entities;
using Data.Interfaces;
using System;
using System.Linq;

namespace Data.Repositories
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly SqlDbContext dbContext;

        public SqlUserRepository(SqlDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User is null");

            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }

        public User GetUser(Func<User, bool> predicate)
        {
            return dbContext.Users.FirstOrDefault(predicate);
        }

        public User GetUserById(int id)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
                throw new ArgumentNullException($"UserId {id} not found");

            return user;
        }

        public void UpdateLastLoginDate(int userId, DateTime dateTime)
        {
            var existingUser = GetUserById(userId);
            existingUser.LastLogin = dateTime;
            dbContext.SaveChanges();
        }
    }
}
