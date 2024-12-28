using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using SV21T1020285.BusinessLayers;
using SV21T1020285.DomainModels;
using System.Web;
namespace SV21T1020285.MeteorShop.Controllers;

public class CartController : Controller
{

    [HttpGet]
    public IActionResult Index()
    {
        var userId = HttpContext.User.FindFirst("UserId")?.Value;

        if(userId != null) {

            int CustomerID = Convert.ToInt32(userId);
            int CartID = 0;

            if (!CartDataService.CheckExistsCart(CustomerID))
            {
                // Nếu không có, thêm giỏ hàng mới
                CartID = CartDataService.AddCart(CustomerID);
            }
            else
            {
                CartID = CartDataService.GetCartIDByCustomerID(CustomerID);
            }
            var cartItems = CartDataService.ListOfCartItems(CartID);
            return View(cartItems);
        }
        return RedirectToAction("Login", "Account");
    }

    [HttpPost]
    public IActionResult Index(string ProductID, string ProductName, string Quantity, string Price, string Photo)
    {
        var userId = HttpContext.User.FindFirst("UserId")?.Value;

        if(userId != null) {
            int CustomerID = Convert.ToInt32(userId);
            int CartID = CartDataService.GetCartIDByCustomerID(CustomerID);
            if(!CartDataService.CheckExistsCartItem(CartID, Convert.ToInt32(ProductID)))
            {
                CartItemSQL cartItem = new CartItemSQL
                {
                    CartID = CartID,
                    ProductID = Convert.ToInt32(ProductID),
                    ProductName = "",
                    Photo = "",
                    Quantity = Convert.ToInt32(Quantity),
                    Price = Convert.ToDecimal(Price)
                };

                CartDataService.AddCartItem(cartItem);
            }
            else
            {
                CartDataService.UpdateCartItem(CartID, Convert.ToInt32(ProductID), Convert.ToInt32(Quantity));
            }

            var CartItems = CartDataService.ListOfCartItems(CartID);
            return View(CartItems);
        }
        
        return RedirectToAction("Login", "Account");
    }

    [HttpGet("Cart/Delete/{CartItemID}")]
    public IActionResult Delete(int CartItemID = 0)
    {
        bool result = CartDataService.DeleteCartItem(CartItemID);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Update(int CartItemID = 0, int Quantity = 0)
    {
        bool result = CartDataService.UpdateQuantityCartItem(CartItemID, Quantity);
        return RedirectToAction("Index");
    }

}
