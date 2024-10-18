using Microsoft.AspNetCore.Mvc;
using SV21T1020285.BusinessLayers;
using SV21T1020285.DomainModels;
namespace MvcMovie.Controllers;

public class ShipperController : Controller
{
     public const int PAGE_SIZE = 10;


    public IActionResult Index(int page = 1, string searchValue = "")
    {
        int rowCount;
        var data = CommonDataService.ListOfShippers(out rowCount, page, PAGE_SIZE, searchValue ?? "");

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
        ViewBag.Title = "Bổ sung người giao hàng";
        var data = new Shipper() {
                ShipperID = 0
        };
        return View("Edit", data);
    }
    public IActionResult Edit(int id = 0) { // public IActionResult Edit(int id) => View();
        ViewBag.Title = "Cập nhật người giao hàng";
        var data = CommonDataService.GetShipper(id);
        if(data == null) return RedirectToAction("Index");
        return View(data);
    }
    [HttpPost]
    public IActionResult Save(Shipper data) {
        //TODO: Kiểm soát dữ liệu đầu vào --> validation
        if(data.ShipperID == 0) {
            CommonDataService.AddShipper(data);
        }
        else {
            CommonDataService.UpdateShipper(data);
        }
        return RedirectToAction("Index");
    }
    public IActionResult Delete(int id) { // public IActionResult Delete(int id) => View();
        if(Request.Method == "POST") {
            CommonDataService.DeleteShipper(id);
            return RedirectToAction("Index");
        }
        var data = CommonDataService.GetShipper(id);
        if(data == null) return RedirectToAction("Index");
        return View(data);
    }
}