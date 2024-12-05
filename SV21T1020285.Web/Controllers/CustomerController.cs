using SV21T1020285.BusinessLayers;
using SV21T1020285.DomainModels;
using SV21T1020285.Web.Models;
using Microsoft.AspNetCore.Mvc;
using SV21T1020285.Web.AppCodes;
using Microsoft.AspNetCore.Authorization;
namespace SV21T1020285.Web.Controllers
{
[Authorize(Roles = $"{WebUserRoles.ADMIN}")] 
    public class CustomerController : Controller
    {

        public const int PAGE_SIZE = 10;
        private const string CUSTOMER_SEARCH_CONDITION = "CustomerSearchCondition";

        public IActionResult Index()
        {
            PaginationSearchInput? condition = ApplicationContext.GetSessionData<PaginationSearchInput>(CUSTOMER_SEARCH_CONDITION);
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
            var data = CommonDataService.ListOfCustomers(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "");
            CustomerSearchResult model = new CustomerSearchResult() {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            ApplicationContext.SetSessionData(CUSTOMER_SEARCH_CONDITION, condition);

            return View(model);
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
            // Validation
            ViewBag.Title = data.CustomerID == 0 ? "Bổ sung khách hàng" : "Cập nhật khách hàng";

            //Kiểm tra dữ liệu đầu vào, nếu như không hợp lệ thì chúng ta tạo ra thoong báo lỗi và lưu giữ nó trong ModelState
            // ModelState.AddModelError(key, message))
            //      - Key: Chuỗi tên lỗi/mã lỗi
            //      - Message: Thông báo lỗi mà ta muốn hiển thị trên view
            if(string.IsNullOrWhiteSpace(data.CustomerName))
                ModelState.AddModelError(nameof(data.CustomerName), "Tên khách hàng không được bỏ trống");
            if(string.IsNullOrWhiteSpace(data.ContactName)) 
                ModelState.AddModelError(nameof(data.ContactName), "Tên giao dịch không được bỏ trống");
            if(string.IsNullOrWhiteSpace(data.Phone))
                ModelState.AddModelError(nameof(data.Phone), "Vui lòng nhập số điện thoại");
            if(string.IsNullOrWhiteSpace(data.Email)) 
                ModelState.AddModelError(nameof(data.Email), "Vui lòng nhập địa chỉ Email");
            if(string.IsNullOrWhiteSpace(data.Address))
                ModelState.AddModelError(nameof(data.Address), "Vui lòng nhập địa chỉ");    
            if(string.IsNullOrWhiteSpace(data.Province))
                ModelState.AddModelError(nameof(data.Province), "Vui lòng chọn Tỉnh/Thành của bạn");
            
            if(!ModelState.IsValid) {
                return View("Edit", data); // Trả dữ liệu về cho view
            }
            if(data.CustomerID == 0){ 
                int id = CommonDataService.AddCustomer(data);
                if(id <= 0) {
                    ModelState.AddModelError(nameof(data.Email), "Email đã tồn tại");
                    return View("Edit", data);
                }
            }
            else {
                bool result = CommonDataService.UpdateCustomer(data);
                if(!result) {
                    ModelState.AddModelError(nameof(data.Email), "Email đã tồn tại");
                    return View("Edit", data);
                }
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
}