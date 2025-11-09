using WebBE1.Models;
using WebBE1.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace WebBE1.Controllers
{
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        //GET: Ordeer//Checkout
        [Authorize]
        public ActionResult Checkout()
        {
            //Kiểm tra giỏ hàng trong session,
            //nếu giỏ hàng rỗng hoặc không có sản phẩm thì chuyển hướng về trang chủ
            var cart = Session["Cart"] as Cart;
            if ( (cart == null || !cart.Items.Any()))
            {
                return RedirectToAction("Index", "Home");
            }
            //Xác thực người dùng đã đăng nhập chưa, nếu chưa thì chuyển hướng tới trang đăng nhập
            var user = db.User.SingleOrDefault(u => u.Username == User.Identity.Name);
            if (user == null) { return RedirectToAction("Login", "Account"); }
            //Lấy thông tin khách hàng từ CSDL, nếu chưa có thì chuyển hướng tới trang Đăng Nhập
            //nếu có rồi thì lấy địa chỉ của khách hàng và gán vào ShippingAddress của Checkout
            var customer = db.Customer.SingleOrDefault(c => c.Username == user.Username);
            if (customer == null) { return RedirectToAction("Login", "Account"); }

            var model = new CheckoutVM
            {
                CartItems = cart.Items.ToList(),
                TotalAmount = cart.Items.Sum(item => item.TotalPrice),
                OrderDate = DateTime.Now,
                ShippingAddress = customer.CustomerAddress,
                CustomerID = customer.CustomerID,
                UserName = customer.Username
            };
            return View(model);
        }

        //Post: Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Checkout(CheckoutVM model)
        {
            if (ModelState.IsValid)
            {
                var cart = Session["Cart"] as Cart;
                //Nếu giỏ hàng rỗng sẽ điều hướng tới trang Home
                if (cart == null || !cart.Items.Any()) { return RedirectToAction("Index", "Home"); }
                //Nếu người dùng chưa đăng nhập sẽ điều hướng tới trang Login
                var user = db.User.SingleOrDefault(u => u.Username == User.Identity.Name);
                if (user == null) { return RedirectToAction("Login", "Account"); }
                //Nếu khách hàng không khớp với tên tên đăng nhập, sẽ điều hướng tới trang Login
                var customer = db.Customer.SingleOrDefault(c =>c.Username == user.Username);
                if (customer == null) { return RedirectToAction("Login", "Account"); }
                //Nếu người dùng chọn thanh toán bằng Paypal, sẽ điều hướng tới trang PaymentWithPaypal
                if (model.PaymentMethod == "Paypal")
                {
                    return RedirectToAction("PaymentWithPaypal", "Paypal", model);
                }
                //Thiết lập PaymentStatus dựa trên PaymentMethod
                string paymentStatus = "Chưa thanht toán";
                switch (model.PaymentMethod)
                {
                    case "Tiền mặt": paymentStatus = "Thanh toán tiền mặt"; break;
                    case "Paypal": paymentStatus = "Thanh toán paypal"; break;
                    case "Mua trước trả sau": paymentStatus = "Trả góp"; break;
                    default: paymentStatus = "Chưa thanh toán"; break;
                }
                //Tạo đơn hàng và chi tiết đơn hàng liên quan
                var order = new Order
                {
                    CustomerID = customer.CustomerID,
                    OrderDate = model.OrderDate,
                    TotalAmount = model.TotalAmount,
                    PaymentStatus = model.PaymentStatus,
                    PaymentMethod = model.PaymentMethod,
                    ShippingAddress = model.ShippingAddress,
                    ShippingMethod = model.ShippingMethod,
                    OrderDetail = cart.Items.Select(item => new OrderDetail
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice,
                    }).ToList()
                };
                //Lưu đơn hàng vào CSDL
                db.Order.Add(order);
                db.SaveChanges();
                //Xóa giỏ hàng sau khi đặt hàng thành công
                Session["Cart"] = null;
                //Điều hướng tới trang xác nhận đơn hàng
                return RedirectToAction("OrderSuccess", new { id = order.OrderID });
            }
            return View(model);
        }

        public ActionResult OrderSuccess(int id)
        {
            var order = db.Order.Include("OrderDetail").SingleOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }    
            return View(order);
        }
    }
}