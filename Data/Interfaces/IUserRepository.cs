using Data.Entities;
using System;
using System.Collections.Generic;

namespace Data.Interfaces
{
    public interface IUserRepository
    {
        User GetUserById(int id);
        User GetUser(Func<User, bool> predicate);
        void CreateUser(User user);
        void UpdateUser(User user);
    }
}
