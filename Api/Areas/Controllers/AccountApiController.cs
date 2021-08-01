using Logic;
using Logic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Areas.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly AccountManager accountManager;

        public AccountApiController(AccountManager accountManager)
        {
            this.accountManager = accountManager;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterRequest request)
        {
            bool isRegistered = accountManager.Register(request);

            if (isRegistered)
                return BadRequest(new ErrorResponse("Register error", "User already was registered"));

            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            var response = accountManager.Login(request.Phone, request.Password);

            if (response == null)
                return BadRequest(new ErrorResponse("login error", "Invalid phone or password"));

            var accessToken = request.Phone + "-token";

            return Ok(accessToken);
        }

        [HttpPost("get-my-info")]
        public ActionResult<UserInfoResponse> GetInfo([FromHeader] string accessToken)
        {
            var response = accountManager.GetInfo(accessToken);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPost("logout")]
        public ActionResult Logout()
        {
            return NoContent();
        }

        /*[HttpGet]
        public ActionResult<User> Get()
        {
            var users = accountManager.GetUsers();

            return Ok(users);
        }*/
    }
}
