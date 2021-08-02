using Data.Entities;
using Data.Interfaces;
using Logic.Models;
using System;

namespace Logic
{
    public class AccountManager
    {
        private readonly IUserRepository userRepository;

        public AccountManager(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public bool Register(RegisterRequest request)
        {
            var isUserExists = userRepository.CheckUserExists(request.Phone, request.Email);

            if (isUserExists)
                return false;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                FIO = request.FIO,
                Phone = request.Phone,
                Email = request.Email,
                Password = hashedPassword
            };

            userRepository.CreateUser(user);

            return true;
        }

        public UserInfoResponse Login(string phone, string password)
        {
            var user = userRepository.GetUserByPhone(phone);

            if (user == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null;

            userRepository.UpdateLastLoginDate(user.Phone, DateTime.UtcNow);

            return new UserInfoResponse
            {
                FIO = user.FIO,
                Email = user.Email,
                Phone = user.Phone,
                LastLogin = user.LastLogin
            };
        }
        
        public UserInfoResponse GetUserInfo(string userPhone)
        {
            var user = userRepository.GetUserByPhone(userPhone);

            if (user == null)
                throw new ArgumentNullException($"User with phone {userPhone} does not exist");

            return new UserInfoResponse
            {
                FIO = user.FIO,
                Email = user.Email,
                Phone = user.Phone,
                LastLogin = user.LastLogin
            };
        }
    }
}
