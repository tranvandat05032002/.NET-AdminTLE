using SV21T1020285.DomainModels;

namespace SV21T1020285.DataLayers
{
    public interface ICartDAL
    {
        // Cart
        int Add(int CustomerID);
        int Get(int CustomerID);
        bool DeleteCart(int CartID);
        bool InUsed(int CustomerID);

        // CartItem
        bool CheckExists(int CartID, int ProductID);
        int Add(CartItemSQL data);
        bool Update(int CartID, int ProductID, int Quantity);
        bool UpdateQuantity(int CartItemID, int Quantity);
        bool Delete(int CartItemID);
        List<CartItemSQL> List(int CartID);
    }
}