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

        public bool CheckUserExists(string phone, string email)
        {
            return dbContext.Users.Any(u => u.Phone == phone || u.Email == email);
        }

        public void CreateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User is null");

            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }

        public User GetUserByPhone(string phone)
        {
            return dbContext.Users.FirstOrDefault(u => u.Phone == phone);
        }

        public void UpdateLastLoginDate(string phone, DateTime dateTime)
        {
            var existingUser = GetUserByPhone(phone);
            existingUser.LastLogin = dateTime;
            dbContext.SaveChanges();
        }
    }
}
