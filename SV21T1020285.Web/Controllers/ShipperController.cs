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
        ViewBag.Title = data.ShipperID == 0 ? "Bổ sung người giao hàng" : "Cập nhật người giao hàng";
        if(string.IsNullOrWhiteSpace(data.ShipperName))
            ModelState.AddModelError(nameof(data.ShipperName), "Tên người giao hàng không được bỏ trống");
        if(string.IsNullOrWhiteSpace(data.Phone)) 
            ModelState.AddModelError(nameof(data.Phone), "Vui lòng nhập số điện thoại người giao hàng");
        if(!ModelState.IsValid) {
                return View("Edit", data);
        }    
        if(data.ShipperID == 0) {
            int id = CommonDataService.AddShipper(data);
            if(id <= 0) {
                ModelState.AddModelError(nameof(data.Phone), "Số điện thoại đã tồn tại");
                return View("Edit", data);
            }
        }
        else {
            bool result = CommonDataService.UpdateShipper(data);
            if(!result) {
                ModelState.AddModelError(nameof(data.Phone), "Số điện thoại đã tồn tại");
                return View("Edit", data);
            }
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