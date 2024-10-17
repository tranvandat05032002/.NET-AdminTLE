using Microsoft.AspNetCore.Mvc;
using SV21T1020285.BusinessLayers;
namespace MvcMovie.Controllers;

public class CategoryController : Controller
{
    public const int PAGE_SIZE = 5;


    public IActionResult Index(int page = 1, string searchValue = "")
    {
        int rowCount;
        var data = CommonDataService.ListOfCategories(out rowCount, page, PAGE_SIZE, searchValue ?? "");

        int pageCount = rowCount / PAGE_SIZE;
        if (rowCount % PAGE_SIZE > 0)
        {
            pageCount += 1;
        }

        ViewBag.Page = page;
        ViewBag.RowCount = rowCount;
        ViewBag.PageCount = pageCount;
        ViewBag.searchValue = searchValue;

        return  View(data);
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