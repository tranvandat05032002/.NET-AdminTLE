using Microsoft.AspNetCore.Mvc;
namespace MvcMovie.Controllers;

public class EmployeeController : Controller
{
    public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhân viên";
            return View("Edit");
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin nhân viên";
            return View();
        }

        public IActionResult Delete(int id = 0)
        {
            return View();
        }
}