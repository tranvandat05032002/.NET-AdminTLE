using SV21T1020285.DataLayers;
using SV21T1020285.DataLayers.SQL_Server;
using SV21T1020285.DomainModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SV21T1020285.BusinessLayers
{
    public static class CartDataService
    {
        private static readonly ICartDAL cartDB;
        static CartDataService()
        {
            cartDB = new CartItemDAL(Configuration.ConnectionString);
        }

        // Cart
        public static int AddCart(int CustomerID)
        {
            return cartDB.Add(CustomerID);
        }

        public static int GetCartIDByCustomerID(int CustomerID)
        {
            return cartDB.Get(CustomerID);
        }

        public static bool CheckExistsCart(int CustomerID)
        {
            return cartDB.InUsed(CustomerID);
        }

        // CartItem
        public static List<CartItemSQL> ListOfCartItems(int CartID)
        {
            return cartDB.List(CartID);
        }

        public static int AddCartItem (CartItemSQL data)
        {
            return cartDB.Add(data);
        }

        public static bool DeleteCartItem (int CartItemID)
        {
            return cartDB.Delete(CartItemID);
        }

        public static bool UpdateCartItem(int CartID, int ProductID, int Quantity)
        {
            return cartDB.Update(CartID, ProductID, Quantity);
        }
        
        public static bool UpdateQuantityCartItem(int CartItemID, int Quantity)
        {
            return cartDB.UpdateQuantity(CartItemID, Quantity);
        }

        public static bool CheckExistsCartItem(int CartID, int ProductID) 
        {
            return cartDB.CheckExists(CartID, ProductID);
        }
    }
}