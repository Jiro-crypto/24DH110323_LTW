using WebBE1.Models;
using WebBE1.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebBE1.Controllers
{
    public class AccountController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        // GET: Admin/Home
        public ActionResult Register()
        {
            return View();
        }

        //POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = db.User.SingleOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("UserName", "Tên đăng nhập này đã tồn tại!");
                    return View(model);
                }


                var user = new WebBE1.Models.User
                {
                    Username = model.Username,
                    Password = model.Password,
                    UserRole = "C"
                };
                db.User.Add(user);
                var customer = new Customer
                {
                    CustomerName = model.CustomerName,
                    CustomerPhone = model.CustomerPhone,
                    CustomerEmail = model.CustomerEmail,
                    CustomerAddress = model.CustomerAddress,
                    Username = model.Username
                };
                db.Customer.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        //Get: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        //POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = db.User.SingleOrDefault(u => u.Username == model.Username
                         && u.Password == model.Password
                         && u.UserRole == "C");
                if (user != null)
                {
                    //Lưu trạng thái đăng nhập vào sesion
                    Session["Username"] = user.Username;
                    Session["UserRole"] = user.UserRole;

                    //lưu trạng thái đăng nhập vào cookie
                    FormsAuthentication.SetAuthCookie(user.Username, false);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                     ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }    
            }    
            return View(model);
        }

        //GET: Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}