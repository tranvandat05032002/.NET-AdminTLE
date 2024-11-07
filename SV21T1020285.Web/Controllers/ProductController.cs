using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020285.BusinessLayers;
using SV21T1020285.DomainModels;
using SV21T1020285.Web.Models;
using SV21T1020285.Web.AppCodes;

namespace MvcMovie.Controllers;

public class ProductController : Controller
{
    public const int PAGE_SIZE = 10;
    private const string PRODUCT_SEARCH_CONDITION = "ProductSearchCondition";
        public IActionResult Index()
        {
            PaginationProductSearchInput? condition = ApplicationContext.GetSessionData<PaginationProductSearchInput>(PRODUCT_SEARCH_CONDITION);
            if(condition == null) {
                condition = new PaginationProductSearchInput() {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    CategoryID = 0,
                    SupplierID = 0,
                    MinPrice = 0m,
                    MaxPrice = 0m
                };
            }
                
    
            return  View(condition);
        }

        public IActionResult Search(PaginationProductSearchInput condition) {
            int rowCount;
            var data = ProductDataService.ListOfProducts(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "", condition.CategoryID, condition.SupplierID, condition.MinPrice, condition.MaxPrice);
            ProductSearchResult model = new ProductSearchResult() {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                CategoryID = condition.CategoryID,
                SupplierID = condition.SupplierID,
                MinPrice = condition.MinPrice,
                MaxPrice = condition.MaxPrice,
                RowCount = rowCount,
                Data = data
            };

            ApplicationContext.SetSessionData(PRODUCT_SEARCH_CONDITION, condition);

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung sản phẩm";
            var data = new Product() {
                    ProductID = 0,
                     Photo = "abc.png",
                    IsSelling = true
            };
            return View("Edit", data);
        }

        public IActionResult Edit(int id = 0)
        {
             ViewBag.Title = "Cập nhật sản phẩm";
            var data = ProductDataService.GetProduct(id);
            if(data == null) return RedirectToAction("Index");
            return View(data);
        }

        [HttpPost]
        public IActionResult Save(Product data, IFormFile? uploadPhoto) {
            ViewBag.Title = data.ProductID == 0 ? "Bổ sung sản phẩm" : "Cập nhật sản phẩm";
            if(string.IsNullOrWhiteSpace(data.ProductName)) 
                ModelState.AddModelError(nameof(data.ProductName), "Tên sản phẩm không được bỏ trống");
            if(string.IsNullOrWhiteSpace(data.ProductDescription))
                data.ProductDescription = "";
            if (data.CategoryID == 0)
                ModelState.AddModelError(nameof(data.CategoryID), "Vui lòng chọn loại hàng");
            if (data.SupplierID == 0)
                ModelState.AddModelError(nameof(data.SupplierID), "Vui lòng chọn nhà cung cấp");
            if(string.IsNullOrWhiteSpace(data.Unit)) 
                ModelState.AddModelError(nameof(data.Unit), "Đơn vị tính không được để trống");
            // TODO: Validation Price
            if(!ModelState.IsValid) {
                return View("Edit", data);
            }

            if(uploadPhoto != null) {
                string fileName = $"{DateTime.Now.Ticks}--{uploadPhoto.FileName}";
                string folder = @"~/images/products"; // Alias Path
                string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images/products", fileName);
                using(var stream = new FileStream(filePath, FileMode.Create)) {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }

            //TODO: Kiểm soát dữ liệu đầu vào --> validation
            if(data.ProductID == 0){ 
                
                int id = ProductDataService.AddProduct(data);
                if(id <= 0) {
                    ModelState.AddModelError(nameof(data.ProductName), "Sản phẩm đã tồn tại");
                    return View("Edit", data);
                }
            }
            else {
                bool result = ProductDataService.UpdateProduct(data);
                if(!result) {
                    ModelState.AddModelError(nameof(data.ProductName), "Sản phẩm đã tồn tại");
                    return View("Edit", data);
                }
            }
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id = 0)
        {
             if(Request.Method == "POST") {
                ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            var data = ProductDataService.GetProduct(id);
            if(data == null) return RedirectToAction("Index");
            return View(data);
        }

        public IActionResult Photo(int id = 0, string method = "", int photoId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    return View();
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh của mặt hàng";
                    return View();
                case "delete":
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }

        public IActionResult Attribute(int id = 0, string method = "", int attributeId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                    return View();
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính của mặt hàng";
                    return View();
                case "delete":
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }
}