using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SV21T1020285.MeteorShop.Models;
using SV21T1020285.BusinessLayers;
using SV21T1020285.DomainModels;
using SV21T1020285.MeteorShop.AppCodes;

namespace SV21T1020285.MeteorShop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public const int PAGE_SIZE = 20;
    private const string PRODUCT_SEARCH_CONDITION = "ProductSearchCondition";

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        PaginationProductSearchInput? condition = ApplicationContext.GetSessionData<PaginationProductSearchInput>(PRODUCT_SEARCH_CONDITION);
        if (condition == null)
        {
            condition = new PaginationProductSearchInput()
            {
                Page = 1,
                PageSize = PAGE_SIZE,
                SearchValue = "",
                CategoryID = 0,
                SupplierID = 0,
                MinPrice = 0m,
                MaxPrice = 0m
            };
        }


        return View(condition);
    }

    [HttpGet]
    public IActionResult Search(PaginationProductSearchInput condition)
    {

        int rowCount;
        var data = ProductDataService.ListOfProducts(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "", condition.CategoryID, condition.SupplierID, condition.MinPrice, condition.MaxPrice);
        PaginationProductSearchResult model = new PaginationProductSearchResult()
        {
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
        //     int rowCount;
        //     var data = ProductDataService.ListOfProducts(out rowCount, 10, 10, "");
        //     PaginationProductSearchResult model = new PaginationProductSearchResult() {
        //         Page = 10,
        //         PageSize = 10,
        //         SearchValue = "",
        //         RowCount = rowCount,
        //         Data = data
        //     };

        // //     // // ApplicationContext.SetSessionData(PRODUCT_SEARCH_CONDITION, condition);

        //     return View(model);
        // //     int rowCount;
        // //     var data = ProductDataService.ListOfProducts(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "", condition.CategoryID, condition.SupplierID, condition.MinPrice, condition.MaxPrice);
        // //     PaginationProductSearchResult model = new PaginationProductSearchResult() {
        // //         Page = condition.Page,
        // //         PageSize = condition.PageSize,
        // //         SearchValue = condition.SearchValue ?? "",
        // //         CategoryID = condition.CategoryID,
        // //         SupplierID = condition.SupplierID,
        // //         MinPrice = condition.MinPrice,
        // //         MaxPrice = condition.MaxPrice,
        // //         RowCount = rowCount,
        // //         Data = data
        // //     };

        // //     ApplicationContext.SetSessionData(PRODUCT_SEARCH_CONDITION, condition);

        // //     return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
