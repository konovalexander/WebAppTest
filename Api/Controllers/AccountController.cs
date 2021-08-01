using Logic;
using Logic.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly AccountManager accountManager;

        public AccountController(AccountManager accountManager)
        {
            this.accountManager = accountManager;
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(registerRequest);
            }

            bool isRegisterSuccess = accountManager.Register(registerRequest);

            if (!isRegisterSuccess)
            {
                ModelState.AddModelError("", "Пользователь с таким телефоном и/или Email уже зарегистрирован");
                return View(registerRequest);
            }

            return View("RegisterSuccessful", registerRequest);
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(CabinetController.Cabinet), "Cabinet");

            return View();
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(loginRequest);
            }

            var userInfo = accountManager.Login(loginRequest.Phone, loginRequest.Password);

            if (userInfo == null)
            {
                ModelState.AddModelError("", "Ошибка авторизации");
                ModelState.AddModelError("Phone", " ");
                return View(loginRequest);
            }

            Authenticate(loginRequest.Phone);

            return RedirectToAction(nameof(CabinetController.Cabinet), "Cabinet");
        }

        private void Authenticate(string userPhone)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userPhone)
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }
    }
}
