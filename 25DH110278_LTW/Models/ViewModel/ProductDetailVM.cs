using System.Collections.Generic;
using PagedList;

namespace _25DH110278_LTW.Models.ViewModel
{
    public class ProductDetailVM
    {
        public Product ProductDetail { get; set; }
        public int Quantity { get; set; } = 1;
        public List<Product> RelatedProducts { get; set; }
        public IPagedList<Product> TopProducts { get; set; }
    }
}