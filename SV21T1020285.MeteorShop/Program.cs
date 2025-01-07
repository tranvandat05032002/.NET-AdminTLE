using SV21T1020285.MeteorShop.AppCodes;
using Microsoft.AspNetCore.Authentication.Cookies;
using SV21T1020285.MeteorShop.AppCodes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews()
.AddMvcOptions(option =>
{
    option.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(option =>
            {
                option.Cookie.Name = "AuthenticationCookie";
                option.LoginPath = "/Account/Login";
                option.AccessDeniedPath = "/Account/AccessDenined";
                option.ExpireTimeSpan = TimeSpan.FromDays(360);
            });
// Thêm policy để kiểm tra quyền truy cập
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MatchUserId", policy =>
        policy.RequireAssertion(context =>
        {
            var httpContext = context.Resource as HttpContext;
            if (httpContext == null)
            {
                return false;
            }
            // Lấy User ID từ claims
            var userId = httpContext.User.FindFirst("UserId")?.Value;
            // Lấy ID từ params treen URL
            var routeId = httpContext.Request.RouteValues["id"]?.ToString();

            return userId != null && routeId != null && userId == routeId;
        })
    );
});

builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(60);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});
var app = builder.Build();

// app.Use(async (context, next) =>
// {
//     try
//     {
//         // Tiếp tục pipeline bình thường
//         await next.Invoke();
//     }
//     catch (System.Security.Cryptography.CryptographicException)
//     {
//         // Xóa cookie không hợp lệ
//         context.Response.Cookies.Delete("AuthenticationCookie");
//         context.Response.Cookies.Delete(".AspNetCore.Session");

//         // Optionally redirect to a login page or handle the error gracefully
//         context.Response.Redirect("/Account/Login");
//     }
// });

// Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// app.UseHttpsRedirection();
// app.UseStaticFiles();

// app.UseRouting();

// app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

ApplicationContext.Configure
(
context: app.Services.GetRequiredService<IHttpContextAccessor>(),
enviroment: app.Services.GetRequiredService<IWebHostEnvironment>()
);
app.Run();
