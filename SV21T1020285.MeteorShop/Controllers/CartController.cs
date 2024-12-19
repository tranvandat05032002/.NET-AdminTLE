using Microsoft.AspNetCore.Mvc;

namespace SV21T1020285.MeteorShop.Controllers;

public class CartController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

}
