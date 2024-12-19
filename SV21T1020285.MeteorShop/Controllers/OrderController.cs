using Microsoft.AspNetCore.Mvc;

namespace SV21T1020285.MeteorShop.Controllers;

public class OrderController : Controller
{
    public IActionResult ProcessOrder()
    {
        return View();
    }

}
