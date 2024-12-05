using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020285.BusinessLayers;
using SV21T1020285.DomainModels;
using SV21T1020285.Web.Models;
using SV21T1020285.Web.AppCodes;
using Microsoft.AspNetCore.Authorization;

namespace SV21T1020285.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.ADMIN}")] 
    public class EmployeeController : Controller
    {
        public const int PAGE_SIZE = 20;
        private const string EMPLOYEE_SEARCH_CONDITION = "EmployeeSearchCondition";
        public IActionResult Index()
            {
            PaginationSearchInput? condition = ApplicationContext.GetSessionData<PaginationSearchInput>(EMPLOYEE_SEARCH_CONDITION);
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
                var data = CommonDataService.ListOfEmployees(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "");
                EmployeeSearchResult model = new EmployeeSearchResult() {
                    Page = condition.Page,
                    PageSize = condition.PageSize,
                    SearchValue = condition.SearchValue ?? "",
                    RowCount = rowCount,
                    Data = data
                };

                ApplicationContext.SetSessionData(EMPLOYEE_SEARCH_CONDITION, condition);

                return View(model);
            }

            public IActionResult Create()
            {
                ViewBag.Title = "Bổ sung nhân viên";
                var data = new Employee()
                {
                    EmployeeID = 0,
                    Photo = "abc.png",
                    IsWorking = true
                };
                return View("Edit", data);
            }

            public IActionResult Edit(int id = 0)
            {
                ViewBag.Title = "Cập nhật thông tin nhân viên";
                var data = CommonDataService.GetEmployee(id);
                if(data == null) {
                    return RedirectToAction("Index");
                }
                return View(data);
            }
            [HttpPost]
            public IActionResult Save(Employee data, string _birthDate, IFormFile? uploadPhoto) {
                ViewBag.Title = data.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật nhân viên";
                if(string.IsNullOrWhiteSpace(data.FullName))
                    ModelState.AddModelError(nameof(data.FullName), "Tên nhân viên không được bỏ trống");
                if(string.IsNullOrWhiteSpace(data.Email)) 
                    ModelState.AddModelError(nameof(data.Email), "Vui lòng nhập địa chỉ Email nhân viên");
                if(string.IsNullOrWhiteSpace(_birthDate)) 
                    ModelState.AddModelError(nameof(_birthDate), "Vui lòng nhập địa chỉ Email nhân viên");
                
                DateTime? d = _birthDate.ToDateTime();
                if(d != null){
                    data.BirthDate = d.Value;
                }
                else {
                    ModelState.AddModelError(nameof(data.BirthDate), "Ngày sinh không hợp lệ");
                }
                if(string.IsNullOrWhiteSpace(data.Phone))
                    data.Phone = "";
                if(string.IsNullOrWhiteSpace(data.Address))
                    data.Address = "";

                if(!ModelState.IsValid) {
                    return View("Edit", data);
                }
                if(uploadPhoto != null) {
                    string fileName = $"{DateTime.Now.Ticks}--{uploadPhoto.FileName}";
                    string folder = @"~/images/employee"; // Alias Path
                    string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images/employee", fileName);
                    using(var stream = new FileStream(filePath, FileMode.Create)) {
                        uploadPhoto.CopyTo(stream);
                    }
                    data.Photo = fileName;
                }
                //TODO: Kiểm soát dữ liệu đầu vào --> validation
                if(data.EmployeeID == 0){ 
                    
                    int id = CommonDataService.AddEmployee(data);
                    if(id <= 0) {
                        ModelState.AddModelError(nameof(data.Email), "Email đã tồn tại");
                        return View("Edit", data);
                    }
                }
                else {
                    bool result = CommonDataService.UpdateEmployee(data);
                    if(!result) {
                        ModelState.AddModelError(nameof(data.Email), "Email đã tồn tại");
                        return View("Edit", data);
                    }
                }
                return RedirectToAction("Index");
            }
            private DateTime? ToDateTime(string input, string formats = "d/M/yyyy;d-M-yyyy;d.M.yyyy") {
                try {
                    return DateTime.ParseExact(input, formats.Split(';'), CultureInfo.InvariantCulture);
                }catch {
                    return null;
                }
            }
            public IActionResult Delete(int id = 0)
            {
                if(Request.Method == "POST") {
                CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
                }
                var data = CommonDataService.GetEmployee(id);
                if(data == null) {
                    return RedirectToAction("Index");
                }
                return View(data);
            }
    }
}