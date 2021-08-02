using Data.Entities;
using System;

namespace Data.Interfaces
{
    public interface IUserRepository
    {
        User GetUserByPhone(string phone);
        bool CheckUserExists(string phone, string email);
        void CreateUser(User user);
        void UpdateLastLoginDate(string phone, DateTime dateTime);
    }
}
