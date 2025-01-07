using SV21T1020285.DomainModels;
namespace SV21T1020285.MeteorShop.Models
{
    public class OrderDetailModel
    {
        public Order? Order { get; set; }
        public required List<OrderDetail> Details { get; set; }
    }
}