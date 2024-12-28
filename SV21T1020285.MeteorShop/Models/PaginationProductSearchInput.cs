namespace SV21T1020285.MeteorShop.Models
{
    public class PaginationProductSearchInput {
        public int Page {get; set;} = 1;
        public int PageSize {get; set;}
        public string SearchValue {get; set;} = "";
        
         public int CategoryID { get; set; } = 0;
        public int SupplierID { get; set; } = 0;
        public decimal MinPrice {get; set;} = 0m;
        public decimal MaxPrice {get; set;} = 0m;
    }
}