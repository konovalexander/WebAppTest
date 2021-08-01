﻿using Data.Entities;
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
            var user = userRepository.GetUser(u => u.Email == request.Email || u.Phone == request.Phone);
            if (user != null)
                return false;

            user = new User
            {
                FIO = request.FIO,
                Phone = request.Phone,
                Email = request.Email,
                Password = request.Password
            };

            userRepository.CreateUser(user);

            return true;
        }

        public UserInfoResponse Login(string phone, string password)
        {
            var user = userRepository.GetUser(u => u.Phone == phone && u.Password == password);

            if (user == null)
                return null;

            userRepository.UpdateUser(user);

            return new UserInfoResponse
            {
                FIO = user.FIO,
                Email = user.Email,
                Phone = user.Phone,
                LastLogin = user.LastLogin
            };
        }

        public UserInfoResponse GetInfo(string token)
        {
            if (token == null)
                return null;

            var phone = token.Split('-')[0];

            var user = userRepository.GetUser(u => u.Phone == phone);

            if (user == null)
                return null;

            return new()
            {
                FIO = user.FIO,
                Phone = user.Phone,
                Email = user.Email,
                LastLogin = user.LastLogin
            };
        }

        public UserInfoResponse GetUserByPhone(string phone)
        {
            var user = userRepository.GetUser(u => u.Phone == phone);

            if (user == null)
                throw new ArgumentNullException($"User with phone {phone} does not exist");

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