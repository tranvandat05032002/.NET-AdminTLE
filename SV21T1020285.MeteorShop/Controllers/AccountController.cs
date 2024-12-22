using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using SV21T1020285.MeteorShop.AppCodes;
using Microsoft.AspNetCore.Authorization;
using SV21T1020285.BusinessLayers;
using SV21T1020285.DomainModels;

namespace SV21T1020285.MeteorShop.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            ViewBag.Email = email;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Error", "Nhập đầy đủ Email và Mật khẩu!");
                return View();
            }

            var userAccount = UserAccountService.Authorize(UserTypes.Customer, email, password);
            if (userAccount == null)
            {
                ModelState.AddModelError("Error", "Email hoặc mật khẩu không đúng!");
                return View();
            }

            WebUserData userData = new WebUserData()
            {
                UserId = userAccount.UserId,
                UserName = userAccount.UserName,
                DisplayName = userAccount.DisplayName,
                Photo = userAccount.Photo,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userData.CreatePrincipal());
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Info(int id = 0)
        {
            ViewBag.Title = "Thông tin cá nhân";
            var data = UserAccountService.GetAccount(id);
            if (data == null)
            {
                return RedirectToAction("Login");
            }
            return View(data);
        }

        [Authorize(Policy = "MatchUserId")]
        public IActionResult Edit(int id = 0)
        {
            var userData = User.GetUserData();
            var userId = userData.UserId;
            ViewBag.Title = "Cập nhật thông tin cá nhân";
            var data = UserAccountService.GetAccount(id);
            if (data == null)
            {
                return RedirectToAction("Login");
            }
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Account data, IFormFile? uploadPhoto)
        {
            // Validation
            ViewBag.Title = data.AccountID == 0 ? "Bổ sung" : "Cập nhật";

            //Kiểm tra dữ liệu đầu vào, nếu như không hợp lệ thì chúng ta tạo ra thoong báo lỗi và lưu giữ nó trong ModelState
            // ModelState.AddModelError(key, message))
            //      - Key: Chuỗi tên lỗi/mã lỗi
            //      - Message: Thông báo lỗi mà ta muốn hiển thị trên view
            if (string.IsNullOrWhiteSpace(data.AccountName))
                ModelState.AddModelError(nameof(data.AccountName), "Tên không được bỏ trống");
            if (string.IsNullOrWhiteSpace(data.ContactName))
                ModelState.AddModelError(nameof(data.ContactName), "Tên giao dịch không được bỏ trống");
            if (string.IsNullOrWhiteSpace(data.Phone))
                ModelState.AddModelError(nameof(data.Phone), "Vui lòng nhập số điện thoại");
            if (string.IsNullOrWhiteSpace(data.Email))
                ModelState.AddModelError(nameof(data.Email), "Vui lòng nhập địa chỉ Email");
            if (string.IsNullOrWhiteSpace(data.Address))
                ModelState.AddModelError(nameof(data.Address), "Vui lòng nhập địa chỉ");
            if (string.IsNullOrWhiteSpace(data.Province))
                ModelState.AddModelError(nameof(data.Province), "Vui lòng chọn Tỉnh/Thành của bạn");

            if (!ModelState.IsValid)
            {
                return View("Edit", data); // Trả dữ liệu về cho view
            }
            if(uploadPhoto != null) {
                string fileName = $"{DateTime.Now.Ticks}--{uploadPhoto.FileName}";
                string folder = @"~/images/customer"; // Alias Path
                string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images/customer", fileName);
                using(var stream = new FileStream(filePath, FileMode.Create)) {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }
            bool result = UserAccountService.Update(data);
            if (!result)
            {
                ModelState.AddModelError(nameof(data.Email), "Email đã tồn tại");
                return View("Edit", data);
            }
            WebUserData userData = new WebUserData()
            {
                UserId = data.AccountID.ToString().Trim(),
                UserName = data.Email,
                DisplayName = data.AccountName,
                Photo = data.Photo,
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userData.CreatePrincipal());
            return RedirectToAction("Index", "Home");
        }

        // public async Task<IActionResult> Login(string username, string password)
        // {
        //     return RedirectToAction("Index", "Home");
        // }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword, string confirmNewPassword)
        {
           
            // Validation cho mật khẩu mới không được để trống và khớp với validation của đăng nhập
            if (string.IsNullOrWhiteSpace(oldPassword))
                ModelState.AddModelError(nameof(oldPassword), "Vui lòng nhập mật khẩu hiện tại");
            if (string.IsNullOrWhiteSpace(newPassword))
                ModelState.AddModelError(nameof(newPassword), "vui lòng nhập mật khẩu mới");
            if (string.IsNullOrWhiteSpace(confirmNewPassword))
                ModelState.AddModelError(nameof(confirmNewPassword), "Vui lòng nhập lại mật khẩu");
            // Kiểm tra mật khẩu cũ phải đúng
            var userData = User.GetUserData();
            var userId = userData.UserId;
            bool checkOldPassword = UserAccountService.VerifyPassword(int.Parse(userId), oldPassword);
            if(!checkOldPassword) {
                ModelState.AddModelError(nameof(oldPassword), "Mật khẩu cũ không chính xác");
            }
            // Kiểm tra mật khẩu mới không được trùng với mật khẩu cũ
            if(newPassword == oldPassword){
                ModelState.AddModelError(nameof(newPassword), "Mật khẩu mới không được trùng với mật khẩu cũ");
            }
            // Mật khẩu mới và nhập lại mật khẩu mới khải khớp nhau
            if(newPassword != confirmNewPassword) {
                ModelState.AddModelError(nameof(confirmNewPassword), "Mật khẩu mới và xác nhận mật khẩu không khớp");
            }
            if (!ModelState.IsValid)
            {
                return View("ChangePassword"); 
            }

            // Cập nhật mật khẩu
            bool result = UserAccountService.UpdatePassword(int.Parse(userId), newPassword);
            if(!result) {
                ModelState.AddModelError("Error", "Cập nhật mật khẩu không thành công");
                return View("ChangePassword"); 
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenined()
        {
            return View();
        }
    }
}
