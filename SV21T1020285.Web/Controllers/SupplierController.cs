using Microsoft.AspNetCore.Mvc;
namespace MvcMovie.Controllers;

public class SupplierController : Controller
{
    public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhà cung cấp";
            return View("Edit");
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin nhà cung cấp";
            return View();
        }

        public IActionResult Delete(int id = 0)
        {
            return View();
        }
}