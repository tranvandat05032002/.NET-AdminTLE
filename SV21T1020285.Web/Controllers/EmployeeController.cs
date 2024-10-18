using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020285.BusinessLayers;
using SV21T1020285.DomainModels;
namespace MvcMovie.Controllers;

public class EmployeeController : Controller
{
    public const int PAGE_SIZE = 10;
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
            // Xu ly ngay
            DateTime? d = ToDateTime(_birthDate);
            if(d != null){
                data.BirthDate = d.Value;
            }
            if(uploadPhoto != null) {
                string fileName = $"{DateTime.Now.Ticks}--{uploadPhoto.FileName}";
                string folder = @"/Users/spiderman/Documents/DHKH_2021-2025/LTUDW/SV21T1020285/SV21T1020285.Web/wwwroot/images/employee";
                string filePath = Path.Combine(folder, fileName);
                using(var stream = new FileStream(filePath, FileMode.Create)) {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }
            //TODO: Kiểm soát dữ liệu đầu vào --> validation
            if(data.EmployeeID == 0){ 
                CommonDataService.AddEmployee(data);
            }
            else {
                CommonDataService.UpdateEmployee(data);
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