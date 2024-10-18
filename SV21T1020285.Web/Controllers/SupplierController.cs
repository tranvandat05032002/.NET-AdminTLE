using Microsoft.AspNetCore.Mvc;
using SV21T1020285.BusinessLayers;
using SV21T1020285.DomainModels;
namespace MvcMovie.Controllers;

public class SupplierController : Controller
{
     public const int PAGE_SIZE = 10;


    public IActionResult Index(int page = 1, string searchValue = "")
    {
        int rowCount;
        var data = CommonDataService.ListOfSuppliers(out rowCount, page, PAGE_SIZE, searchValue ?? "");

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
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhà cung cấp";
            var data = new Supplier() {
                SupplierID = 0
            };
            return View("Edit", data);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin nhà cung cấp";
            var data = CommonDataService.GetSupplier(id);
            if(data == null) return RedirectToAction("Index");
            return View(data);
        }
        [HttpPost]
        public IActionResult Save(Supplier data) {
            //TODO: Kiểm soát dữ liệu đầu vào --> validation
            if(data.SupplierID == 0) {
                CommonDataService.AddSupplier(data);
            }
            else {
                CommonDataService.UpdateSupplier(data);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {
            if(Request.Method == "POST") {
                CommonDataService.DeleteSupplier(id);
                return RedirectToAction("Index");
            }
            var data = CommonDataService.GetSupplier(id);
            if(data == null) return RedirectToAction("Index");
            return View(data);
        }
}