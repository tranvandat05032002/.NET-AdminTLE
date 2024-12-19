using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Authentication.Cookies;
// using Microsoft.AspNetCore.Authentication;

namespace SV21T1020285.MeteorShop.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Info()
        {
            return View("Info");
        }

        // public async Task<IActionResult> Login(string username, string password)
        // {
        //     return RedirectToAction("Index", "Home");
        // }

        public async Task<IActionResult> Logout()
        {
            // HttpContext.Session.Clear();
            // await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        public IActionResult AccessDenined()
        {
            return View();
        }
    }
}
