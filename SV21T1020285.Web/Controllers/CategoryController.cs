using SV21T1020285.BusinessLayers;
using SV21T1020285.DomainModels;
using SV21T1020285.Web.Models;
using Microsoft.AspNetCore.Mvc;
using SV21T1020285.Web.AppCodes;
namespace MvcMovie.Controllers;

public class CategoryController : Controller
{
    public const int PAGE_SIZE = 5;
    private const string CATEGORY_SEARCH_CONDITION = "CategorySearchCondition";
    public IActionResult Index()
    {
        PaginationSearchInput? condition = ApplicationContext.GetSessionData<PaginationSearchInput>(CATEGORY_SEARCH_CONDITION);
    if(condition == null)
        condition = new PaginationSearchInput() {
            Page = 1,
            PageSize = PAGE_SIZE,
            SearchValue = ""
        };

    return  View(condition);
    }

    public IActionResult Search(PaginationSearchInput condition) {
        int rowCount;
        var data = CommonDataService.ListOfCategories(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "");
        CategorySearchResult model = new CategorySearchResult() {
            Page = condition.Page,
            PageSize = condition.PageSize,
            SearchValue = condition.SearchValue ?? "",
            RowCount = rowCount,
            Data = data
        };

        ApplicationContext.SetSessionData(CATEGORY_SEARCH_CONDITION, condition);

        return View(model);
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