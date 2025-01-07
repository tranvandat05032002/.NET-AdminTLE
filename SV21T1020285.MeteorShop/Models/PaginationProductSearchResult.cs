using SV21T1020285.DomainModels;
namespace SV21T1020285.MeteorShop.Models
{
    public abstract class PaginationSearchResult {
        public int Page {get; set;}
        public int PageSize {get; set;}
        public string SearchValue {get; set;} = "";
        public int RowCount {get; set;}
        public int PageCount {
            get 
            {
                if(PageSize == 0) 
                    return 1;
                
                int c = RowCount / PageSize;
                if(RowCount % PageSize > 0) 
                    c += 1;

                return c;
            }
        }
    }
    
    public class PaginationProductSearchResult
    {
        public int Page {get; set;}
        public int PageSize {get; set;}
        public string SearchValue {get; set;} = "";
        public int RowCount {get; set;}
        public int PageCount {
            get 
            {
                if(PageSize == 0) 
                    return 1;
                
                int c = RowCount / PageSize;
                if(RowCount % PageSize > 0) 
                    c += 1;

                return c;
            }
        }

        public required List<Product> Data {get; set;}

        // Query có Category và Supplier
        public int CategoryID { get; set; } = 0;
        public int SupplierID {get; set;}
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }

    public class OrderSearchResult : PaginationSearchResult {
        public int Status { get; set; } = 0;
        public string TimeRange { get; set; } = "";
        public List<Order> Data { get; set; } = new List<Order>();
    }
}