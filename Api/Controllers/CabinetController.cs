using Logic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class CabinetController : Controller
    {
        private readonly AccountManager accountManager;

        public CabinetController(AccountManager accountManager)
        {
            this.accountManager = accountManager;
        }

        [HttpGet("cabinet")]
        [Authorize]
        public IActionResult Cabinet()
        {
            var userInfo = accountManager.GetUserByPhone(User.Identity.Name);

            return View(userInfo);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(AccountController.Login), "Account");
        }
    }
}
