using Microsoft.AspNetCore.Mvc;
namespace Sv21T1020285.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}
