
namespace SV21T1020285.Web.Models
{
        public class PaginationProductSearchInput : PaginationSearchInput {
        /// <summary>
        /// ID của Category (nếu có)
        /// </summary>
        public int CategoryID { get; set; } = 0;

        /// <summary>
        /// ID của Supplier (nếu có)
        /// </summary>
        public int SupplierID { get; set; } = 0;

        public decimal MinPrice {get; set;} = 0m;
        public decimal MaxPrice {get; set;} = 0m;
    }
}