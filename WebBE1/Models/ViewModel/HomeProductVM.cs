using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBE1.Models.ViewModel
{
    public class HomeProductVM
    {
        //tiêu chí để search theo tên, mô tả sp
        //hoặc loại sản phẩm
        public string SearchTerm { get; set; }

        //Các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; } //Trang hiện tại
        public int PageSize { get; set; } = 5; //Số mục trên mỗi trang

        //danh sách sản phẩm nổi bật
        public List<Product> FeaturedProducts { get; set; }

        //danh sách sản phẩm mới đã phân trang
        public PagedList.IPagedList<Product> NewProducts { get; set; }
    }
}