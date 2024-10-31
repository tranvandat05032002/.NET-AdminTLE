using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020285.BusinessLayers;
using SV21T1020285.DomainModels;
using SV21T1020285.Web.AppCodes;
namespace MvcMovie.Controllers;

public class EmployeeController : Controller
{
    public const int PAGE_SIZE = 20;
    public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount;
            var data = CommonDataService.ListOfEmployees(out rowCount, page, PAGE_SIZE, searchValue ?? "");

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
                string folder = @"/Users/spiderman/Documents/DHKH_2021-2025/LTUDW/SV21T1020285/SV21T1020285.Web/wwwroot/images/employee";
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