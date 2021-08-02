using Logic;
using Logic.JwtServices;
using Logic.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Areas.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly AccountManager accountManager;
        private readonly JwtAuthenticateManager jwtAuthManager;

        public AccountApiController(AccountManager accountManager, JwtAuthenticateManager jwtAuthManager)
        {
            this.accountManager = accountManager;
            this.jwtAuthManager = jwtAuthManager;
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

            var token = jwtAuthManager.Authenticate(loginRequest.Phone);

            return Ok(token);
        }

        [HttpGet("get-my-info")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<UserInfoResponse> GetInfo()
        {
            var isAuthenticated = jwtAuthManager.CheckAuthenticateUser(HttpContext.User);

            if (isAuthenticated)
            {
                var userPhone = HttpContext.User.FindFirstValue(ClaimTypes.MobilePhone);
                var response = accountManager.GetUserInfo(userPhone);

                if (response != null)
                    return Ok(response);
            }

            return Unauthorized();
        }

        [HttpPost("logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult Logout()
        {
            var isAuthenticated = jwtAuthManager.CheckAuthenticateUser(HttpContext.User);

            if (isAuthenticated)
            {
                var userPhone = HttpContext.User.FindFirstValue(ClaimTypes.MobilePhone);
                jwtAuthManager.Unauthorize(userPhone);

                return Ok();
            }

            return Unauthorized();
        }
    }
}
