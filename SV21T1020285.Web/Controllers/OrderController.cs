using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020285.BusinessLayers;
using SV21T1020285.DomainModels;
using SV21T1020285.Web.Models;
using SV21T1020285.Web.AppCodes;

public class OrderController : Controller
{
        public const int PAGE_SIZE = 10;
        private const string ORDER_SEARCH_CONDITION = "OrderSearchCondition";
        public IActionResult Index()
        {
            PaginationOrderSearchInput? condition = ApplicationContext.GetSessionData<PaginationOrderSearchInput>(ORDER_SEARCH_CONDITION);
            if(condition == null) {
                var cultureInfo = new CultureInfo("en-GB");
                condition = new PaginationOrderSearchInput() {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    Status = 0,
                    TimeRange = $"{DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy", cultureInfo)} - {DateTime.Today.ToString("dd/MM/yyyy", cultureInfo)}"
                };
            }
            return  View(condition);
        }

        public IActionResult Search(PaginationOrderSearchInput condition) {
            int rowCount;
            var data = OrderDataService.ListOrders(out rowCount, condition.Page, condition.PageSize, condition.Status, condition.FromTime, condition.ToTime, condition.SearchValue ?? "");
            OrderSearchResult model = new OrderSearchResult() {
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
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Details(int id = 0)
        {
            var order = OrderDataService.GetOrder(id);
            if(order == null) 
                return RedirectToAction("Index");
            var details = OrderDataService.ListOrderDetails(id);
            var model = new OrderDetailModel()
            {
                Order = order,
                Details = details
            };
            return View(model);
        }

        public IActionResult EditDetail(int id = 0, int productId = 0)
        {
            return View();
        }

        public IActionResult Shipping(int id = 0)
        {
            return View();
        }
}