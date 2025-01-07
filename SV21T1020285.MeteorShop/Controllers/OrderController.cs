using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SV21T1020285.DomainModels;
using SV21T1020285.BusinessLayers;
using SV21T1020285.MeteorShop.Models;
using SV21T1020285.MeteorShop.AppCodes;

namespace SV21T1020285.MeteorShop.Controllers;
[Authorize]
public class OrderController : Controller
{
    public const int PAGE_SIZE = 20;
    private const string ORDER_SEARCH_CONDITION = "OrderSearchCondition";

    public IActionResult Index()
    {
        PaginationOrderSearchInput? condition = ApplicationContext.GetSessionData<PaginationOrderSearchInput>(ORDER_SEARCH_CONDITION);
        if (condition == null)
        {
            var cultureInfo = new CultureInfo("en-GB");
            condition = new PaginationOrderSearchInput()
            {
                Page = 1,
                PageSize = PAGE_SIZE,
                SearchValue = "",
                Status = 0,
                TimeRange = $"{DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy", cultureInfo)} - {DateTime.Today.ToString("dd/MM/yyyy", cultureInfo)}"
            };
        }
        return View(condition);
    }
    public IActionResult Search(PaginationOrderSearchInput condition)
    {
        int rowCount;
        var userId = HttpContext.User.FindFirst("UserId")?.Value;
        int customerID = Convert.ToInt32(userId);
        var data = OrderDataService.ListOrdersCustomer(customerID, out rowCount, condition.Page, condition.PageSize, condition.Status, condition.FromTime, condition.ToTime, condition.SearchValue ?? "");
        OrderSearchResult model = new OrderSearchResult()
        {
            Page = condition.Page,
            PageSize = condition.PageSize,
            SearchValue = condition.SearchValue ?? "",
            Status = condition.Status,
            TimeRange = condition.TimeRange,
            RowCount = rowCount,
            Data = data
        };

        ApplicationContext.SetSessionData(ORDER_SEARCH_CONDITION, condition);
        return View(model);
    }

    [HttpGet]
    public IActionResult ProcessOrder(int id = 0)
    {
        var order = OrderDataService.GetOrder(id);
        var userId = HttpContext.User.FindFirst("UserId")?.Value;
        int currentCustomerID = Convert.ToInt32(userId);
        if (order == null || order.CustomerID != currentCustomerID)
            return RedirectToAction("AccessDenined", "Account");
        var details = OrderDataService.ListOrderDetails(id);
        var model = new OrderDetailModel()
        {
            Order = order,
            Details = details
        };
        return View(model);
    }

    public IActionResult Cancel(int id = 0)
    {
        if (id != 0)
        {
            OrderDataService.CancelOrder(id);
            return RedirectToAction("Index", new { id = id });
        }
        return RedirectToAction("Login", "Account");
    }
}
