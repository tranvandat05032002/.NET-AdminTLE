using SV21T1020285.DomainModels;

namespace SV21T1020285.DataLayers
{
    public interface ICartDAL
    {
        // Cart
        int Add(int CustomerID);
        bool InUsed(int CustomerID);

        // CartItem
        bool CheckExists(int CartID, int ProductID);
        bool Update(int CartID, int Quantity);
        int Add(CartItemSQL data);
        List<CartItemSQL> List(int CartID);
    }
}