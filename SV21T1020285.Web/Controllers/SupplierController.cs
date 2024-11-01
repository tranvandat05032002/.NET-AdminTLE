using Microsoft.AspNetCore.Mvc;
using SV21T1020285.BusinessLayers;
using SV21T1020285.DomainModels;
using SV21T1020285.Web.AppCodes;
using SV21T1020285.Web.Models;
namespace MvcMovie.Controllers;

public class SupplierController : Controller
{
     public const int PAGE_SIZE = 10;
      private const string SUPPLIER_SEARCH_CONDITION = "SupplierSearchCondition";

    public IActionResult Index()
    {
        PaginationSearchInput? condition = ApplicationContext.GetSessionData<PaginationSearchInput>(SUPPLIER_SEARCH_CONDITION);
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
        var data = CommonDataService.ListOfSuppliers(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "");
        SupplierSearchResult model = new SupplierSearchResult() {
            Page = condition.Page,
            PageSize = condition.PageSize,
            SearchValue = condition.SearchValue ?? "",
            RowCount = rowCount,
            Data = data
        };

        ApplicationContext.SetSessionData(SUPPLIER_SEARCH_CONDITION, condition);

        return View(model);
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
            ViewBag.Title = data.SupplierID == 0 ? "Bổ sung nhà cung cấp" : "Cập nhật nhà cung cấp";
            if(string.IsNullOrWhiteSpace(data.SupplierName))
                ModelState.AddModelError(nameof(data.SupplierName), "Tên nhà cung cấp không được bỏ trống");
            if(string.IsNullOrWhiteSpace(data.ContactName)) 
                ModelState.AddModelError(nameof(data.ContactName), "Tên giao dịch không được bỏ trống");
            if(string.IsNullOrWhiteSpace(data.Phone))
                ModelState.AddModelError(nameof(data.Phone), "Vui lòng nhập số điện thoại");
            if(string.IsNullOrWhiteSpace(data.Email)) 
                ModelState.AddModelError(nameof(data.Email), "Vui lòng nhập địa chỉ Email nhà cung cấp");
            if(string.IsNullOrWhiteSpace(data.Address))
                ModelState.AddModelError(nameof(data.Address), "Vui lòng nhập địa chỉ");
            if(string.IsNullOrWhiteSpace(data.Province))
                ModelState.AddModelError(nameof(data.Province), "Vui lòng chọn Tỉnh/Thành của bạn");
            
            if(!ModelState.IsValid) 
                return View("Edit", data); // Trả dữ liệu về cho view
            if(data.SupplierID == 0) {
                int id = CommonDataService.AddSupplier(data);
                if(id <= 0) {
                    ModelState.AddModelError(nameof(data.Email), "Email đã tồn tại");
                    return View("Edit", data);
                }
            }
            else {
                
                bool result = CommonDataService.UpdateSupplier(data);
                if(!result) {
                    ModelState.AddModelError(nameof(data.Email), "Email đã tồn tại");
                    return View("Edit", data);
                }
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