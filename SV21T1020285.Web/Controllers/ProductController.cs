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
            ViewBag.Title = "Bổ sung mặt hàng";
            return View("Edit");
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin mặt hàng";
            return View();
        }

        public IActionResult Delete(int id = 0)
        {
            return View();
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