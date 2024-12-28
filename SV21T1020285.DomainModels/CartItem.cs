namespace SV21T1020285.DomainModels
{
    public class CartItemSQL {
        public int CartItemID { get; set; }
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public string ProductName {get; set;}
        public string Photo {get; set;}
        public int Quantity { get; set; }
        public decimal Price { get; set; }
       public decimal TotalPrice 
       {
            get
            {
                return Quantity * Price;
            } 
       }
    }
}