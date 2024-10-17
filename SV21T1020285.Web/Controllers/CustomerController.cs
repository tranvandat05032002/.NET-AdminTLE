using Microsoft.AspNetCore.Mvc;
using SV21T1020285.BusinessLayers;
using SV21T1020285.DomainModels;
namespace MvcMovie.Controllers;

public class CustomerController : Controller
{

    public const int PAGE_SIZE = 10;


    public IActionResult Index(int page = 1, string searchValue = "")
    {
        int rowCount;
        var data = CommonDataService.ListOfCustomers(out rowCount, page, PAGE_SIZE, searchValue ?? "");

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
        ViewBag.Title = "Bổ sung khách hàng";
        var data = new Customer()
        {
            CustomerID = 0,
            IsLocked = false
        };
        return View("Edit", data);
    }
    public IActionResult Edit(int id = 0) { // public IActionResult Edit(int id) => View();
        ViewBag.Title = "Cập nhật khách hàng";
        var data = CommonDataService.GetCustomer(id);
        if(data == null) {
            return RedirectToAction("Index");
        }
        return View(data);
    }
    [HttpPost]
    public IActionResult Save(Customer data) {
        //TODO: Kiểm soát dữ liệu đầu vào --> validation
        if(data.CustomerID == 0){ 
            CommonDataService.AddCustomer(data);
        }
        else {
            CommonDataService.UpdateCustomer(data);
        }
        return RedirectToAction("Index");
    }
    public IActionResult Delete(int id = 0) { // public IActionResult Delete(int id) => View();
        if(Request.Method == "POST") {
            CommonDataService.DeleteCustomer(id);
            return RedirectToAction("Index");
        }
        var data = CommonDataService.GetCustomer(id);
        if(data == null) {
            return RedirectToAction("Index");
        }
        return View(data);
    }
}