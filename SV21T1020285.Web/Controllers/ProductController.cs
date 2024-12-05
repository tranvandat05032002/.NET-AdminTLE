using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020285.BusinessLayers;
using SV21T1020285.DomainModels;
using SV21T1020285.Web.Models;
using SV21T1020285.Web.AppCodes;
using Microsoft.AspNetCore.Authorization;

namespace SV21T1020285.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.ADMIN}, {WebUserRoles.MANAGER}")] 
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
                    return RedirectToAction("Edit", new{id = id});
                }
                else {
                    ProductDataService.UpdateProduct(data);
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

            [HttpPost]
            public IActionResult SavePhoto(ProductPhoto data, IFormFile? uploadPhoto)
            {
                ViewBag.Title = (data.PhotoID == 0 ? "Bổ sung" : "Thay đổi ảnh") + " cho mặt hàng";


                // Xử lý với ảnh
                if (uploadPhoto == null)
                {
                    if (data.Photo == null)
                    {
                        ModelState.AddModelError(nameof(data.Photo), "Vui lòng thêm ảnh mặt hàng");
                        return View("Photo", data);
                    }
                }       
                else 
                {
                    string fileName = $"{DateTime.Now.Ticks}--{uploadPhoto.FileName}";
                    string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images/products", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        uploadPhoto.CopyTo(stream);
                    }
                    data.Photo = fileName;
                }

                if (data.PhotoID == 0) {
                    ProductDataService.AddPhoto(data);
                } 
                else
                {
                    ProductDataService.UpdatePhoto(data);
                }

                var product = ProductDataService.GetProduct(data.ProductID);
                return View("Edit", product);
            }

            public IActionResult Photo(int id = 0, string method = "", int photoId = 0)
            {
                ProductPhoto data;
                switch (method)
                {
                    case "add":
                        ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                        data = new ProductPhoto() {
                            PhotoID = 0,
                            ProductID = id,
                            IsHidden = false
                        };
                        return View(data);
                    case "edit":
                        ViewBag.Title = "Thay đổi ảnh của mặt hàng";
                        data = ProductDataService.GetProductPhoto(photoId);
                        if(data == null) return RedirectToAction("Index");
                        return View(data);
                    case "delete":
                        ProductDataService.DeletePhoto(photoId);
                        return RedirectToAction("Edit", new { id = id });
                    default:
                        return RedirectToAction("Index");
                }
            }

            public IActionResult Attribute(int id = 0, string method = "", int attributeId = 0)
            {
                ProductAttribute data;
                switch (method)
                {
                    case "add":
                        ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                        data = new ProductAttribute() {
                            ProductID = id,
                            AttributeID = 0
                        };
                        return View(data);

                    case "edit":
                        ViewBag.Title = "hay đổi thuộc tính của mặt hàng";
                        data = ProductDataService.GetProductAttribute(attributeId);
                        if(data == null) return RedirectToAction("Index");
                        return View(data);
                    case "delete":
                        ProductDataService.DeleteAttribute(attributeId);
                        return RedirectToAction("Edit", new { id = id });
                    default:
                        return RedirectToAction("Index");
                }
            }
            [HttpPost]
            public IActionResult SaveAttribute(ProductAttribute data)
            {
                ViewBag.Title = (data.AttributeID == 0 ? "Bổ sung" : "Thay đổi thuộc tính") + " cho mặt hàng";

                if (string.IsNullOrWhiteSpace(data.AttributeName))
                {
                    ModelState.AddModelError(nameof(data.AttributeName), "Vui lòng nhập tên thuộc tính");
                }
                if (string.IsNullOrWhiteSpace(data.DisplayOrder)) {
                    ModelState.AddModelError(nameof(data.DisplayOrder), "Thứ tự hiển thị không hợp lệ");
                }
                if (string.IsNullOrWhiteSpace(data.AttributeValue))
                {
                    ModelState.AddModelError(nameof(data.AttributeValue), "Thuộc tính không được để trống");
                }
                // TODO: Xử lý displayOrder

                if (!ModelState.IsValid)
                {
                    return View("Attribute", data);
                }

                if (data.AttributeID == 0)
                {
                    long id = ProductDataService.AddAttribute(data);
                    Console.WriteLine(data.ProductID);
                    if (id < 0)
                    {
                        ModelState.AddModelError(nameof(data.AttributeName), "Tên thuộc tính đã được sử dụng");
                        return View("Attribute", data);
                    }
                }
                else
                {
                    bool result = ProductDataService.UpdateAttribute(data);
                    if (!result)
                    {
                        ModelState.AddModelError(nameof(data.AttributeName), "Tên thuộc tính đã được sử dụng");
                        return View("Attribute", data);
                    }
                }

                var product = ProductDataService.GetProduct(data.ProductID);
                return View("Edit", product);
            }
    }
}