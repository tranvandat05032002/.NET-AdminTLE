using Microsoft.AspNetCore.Mvc;
namespace MvcMovie.Controllers;

public class CustomerController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Create() {
        ViewBag.Title = "Bổ sung khách hàng";
        return View("Edit");
    }
    public IActionResult Edit(int id = 0) { // public IActionResult Edit(int id) => View();
        ViewBag.Title = "Cập nhật khách hàng";
        return View();
    }
    public IActionResult Delete(int id) { // public IActionResult Delete(int id) => View();
        return View();
    }
}