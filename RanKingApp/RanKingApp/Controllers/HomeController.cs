using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RanKingApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Session["UserId"] = 1;
            Session["email"] = "syed.ahsam@gmail.com";
            return View();
        }
    }
}