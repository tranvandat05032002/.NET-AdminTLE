using Microsoft.AspNetCore.Mvc;
namespace MvcMovie.Controllers;

public class CategoryController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
     public IActionResult Create() {
        ViewBag.Title = "Bổ sung loại hàng";
        return View("Edit");
    }
    public IActionResult Edit(int id = 0) { // public IActionResult Edit(int id) => View();
        ViewBag.Title = "Cập nhật loại hàng";
        return View();
    }
    public IActionResult Delete(int id) { // public IActionResult Delete(int id) => View();
        return View();
    }
}