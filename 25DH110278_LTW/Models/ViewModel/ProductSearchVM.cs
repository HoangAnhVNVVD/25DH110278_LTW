using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace _25DH110278_LTW.Models.ViewModel
{
    public class ProductSearchVM
    {
        // Tiêu chí để search theo tên, mô tả sp hoặc loại sản phẩm (Slide 39)
        public string SearchTerm { get; set; }

        // Các tiêu chí để search theo giá (Slide 42)
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // Thứ tự sắp xếp (Slide 45)
        public string SortOrder { get; set; }

        // Các thuộc tính hỗ trợ phân trang (Slide 48)
        public int PageNumber { get; set; } // Trang hiện tại
        public int PageSize { get; set; } = 10; // Số sản phẩm mỗi trang

        // Danh sách Sản phẩm đã phân trang (Slide 48)
        public IPagedList<Product> Products { get; set; }
    }
}