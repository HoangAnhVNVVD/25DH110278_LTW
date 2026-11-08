using System.Collections.Generic;
using PagedList;

namespace _25DH110278_LTW.Models.ViewModel
{
    public class HomeProductVM
    {
        public string SearchTerm { get; set; }
        public List<Product> FeaturedProducts { get; set; }
        public IPagedList<Product> NewProducts { get; set; }
    }
}