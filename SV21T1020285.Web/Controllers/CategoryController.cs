using Microsoft.AspNetCore.Mvc;
using SV21T1020285.BusinessLayers;
using SV21T1020285.DomainModels;
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
        var data = new Category() {
            CategoryID = 0
        };
        return View("Edit", data);
    }
    [HttpPost]
    public IActionResult Save(Category data) {
        ViewBag.Title = data.CategoryID == 0 ? "Bổ sung loại hàng" : "Cập nhật loại hàng";
        if(string.IsNullOrWhiteSpace(data.CategoryName))
            ModelState.AddModelError(nameof(data.CategoryName), "Tên loại hàng không được bỏ trống");
        if(!ModelState.IsValid) {
            return View("Edit", data);
        }
        if(data.CategoryID == 0){ 
            int id = CommonDataService.AddCategory(data);
            if(id <= 0) {
                ModelState.AddModelError(nameof(data.CategoryName), "Tên loại hàng đã tồn tại");
                return View("Edit", data);
            }
        }
        else {
            bool result = CommonDataService.UpdateCategory(data);
            if(!result) {
                ModelState.AddModelError(nameof(data.CategoryName), "Tên loại hàng đã tồn tại");
                return View("Edit", data);
            }
        }
        return RedirectToAction("Index");
    }
    public IActionResult Edit(int id = 0) { // public IActionResult Edit(int id) => View();
        ViewBag.Title = "Cập nhật loại hàng";
        var data = CommonDataService.GetCategory(id);
        if(data == null) return RedirectToAction("Index");
        return View(data);
    }
    public IActionResult Delete(int id) { // public IActionResult Delete(int id) => View();
        if(Request.Method == "POST") {
            CommonDataService.DeleteCategory(id);
            return RedirectToAction("Index");
        }
        var data = CommonDataService.GetCategory(id);
        if(data == null) return RedirectToAction("Index");
        return View(data);
    }
}