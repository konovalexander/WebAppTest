using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Logic.JwtServices
{
    public class JwtAuthenticateManager
    {
        private readonly IOptions<AuthOptions> authOptions;
        private readonly HashStorage hashStorage;

        public JwtAuthenticateManager(IOptions<AuthOptions> authOptions, HashStorage hashStorage)
        {
            this.authOptions = authOptions;
            this.hashStorage = hashStorage;
        }

        public bool CheckAuthenticateUser(ClaimsPrincipal claims)
        {
            var userPhone = claims.FindFirst(ClaimTypes.MobilePhone).Value;
            var userHash = claims.FindFirst(ClaimTypes.Hash).Value;

            var hash = hashStorage.GetHash(userPhone);

            if (hash == userHash)
                return true;

            return false;
        }

        public string Authenticate(string userPhone)
        {
            var hash = GenerateUserHash();
            var token = GenerateToken(userPhone, hash);
            hashStorage.SetHash(userPhone, hash);

            return token;
        }

        public void Unauthorize(string userPhone)
        {
            var newHash = GenerateUserHash();
            hashStorage.SetHash(userPhone, newHash);
        }

        private string GenerateToken(string phone, string hash)
        {
            var authParams = authOptions.Value;
            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.MobilePhone, phone),
                new Claim(ClaimTypes.Hash, hash)
            };

            var token = new JwtSecurityToken(
                authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(authParams.Lifetime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateUserHash() => BCrypt.Net.BCrypt.HashString(DateTime.Now.ToString());
    }
}