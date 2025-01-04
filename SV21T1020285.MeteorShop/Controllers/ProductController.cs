using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020285.BusinessLayers;

namespace SV21T1020285.MeteorShop.Controllers;

public class ProductController : Controller
{
    // [HttpGet("Product/Info/{id}")]
   public IActionResult Info(int id = 0)
    {
        // var userId = HttpContext.User.FindFirst("UserId")?.Value;

        if(id != null) {
            var data = ProductDataService.GetProduct(id);
            if (data == null)
            {
                return RedirectToAction("Login");
            }
            return View(data);
        }
        return View("Info");
    }


}
