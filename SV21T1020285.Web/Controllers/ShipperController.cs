using Microsoft.AspNetCore.Mvc;
namespace MvcMovie.Controllers;

public class ShipperController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
     public IActionResult Create() {
        ViewBag.Title = "Bổ sung người giao hàng";
        return View("Edit");
    }
    public IActionResult Edit(int id = 0) { // public IActionResult Edit(int id) => View();
        ViewBag.Title = "Cập nhật người giao hàng";
        return View();
    }
    public IActionResult Delete(int id) { // public IActionResult Delete(int id) => View();
        return View();
    }
}