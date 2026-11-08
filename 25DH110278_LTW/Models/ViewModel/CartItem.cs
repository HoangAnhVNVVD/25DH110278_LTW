namespace _25DH110278_LTW.Models.ViewModel
{
    public class CartItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ProductImage { get; set; }
        public string Category { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}