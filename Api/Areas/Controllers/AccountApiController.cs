using Logic;
using Logic.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Areas.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly AccountManager accountManager;
        private readonly IOptions<AuthOptions> authOptions;

        public AccountApiController(AccountManager accountManager, IOptions<AuthOptions> authOptions)
        {
            this.accountManager = accountManager;
            this.authOptions = authOptions;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterRequest registerRequest)
        {
            bool isRegisterSuccess = accountManager.Register(registerRequest);

            if (!isRegisterSuccess)
                return BadRequest(new ErrorResponse("Register error", "User already was registered"));

            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var response = accountManager.Login(loginRequest.Phone, loginRequest.Password);

            if (response == null)
                return BadRequest(new ErrorResponse("login error", "Invalid phone or password"));

            var accessToken = GenerateToken(loginRequest.Phone);

            return Ok(new { access_token = accessToken });

        }

        [HttpPost("get-my-info")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<UserInfoResponse> GetInfo([FromHeader] string accessToken)
        {
            var userPhone = User.FindFirstValue(ClaimTypes.MobilePhone);

            var response = accountManager.GetUserByPhone(userPhone);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPost("logout")]
        public ActionResult Logout()
        {
            return NoContent();
        }

        private string GenerateToken(string phone)
        {
            var authParams = authOptions.Value;

            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.MobilePhone, phone)
            };

            var token = new JwtSecurityToken(
                authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(authParams.Lifetime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
