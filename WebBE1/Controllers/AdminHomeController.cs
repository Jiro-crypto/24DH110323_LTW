using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBE1.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: AdminHome
        public ActionResult Index()
        {
            return View();
        }
    }
}