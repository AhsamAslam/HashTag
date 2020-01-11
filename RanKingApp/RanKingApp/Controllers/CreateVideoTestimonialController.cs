using RankMonster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RankMonster.Controllers
{
    public class CreateVideoTestimonialController : Controller
    {
        //
        // GET: /CreateVideoTestimonial/
        public ActionResult Index()
        {
            if (Session["UserId"] != null && Session["email"] != null)
            {
                DAL obj = new DAL();
                var model = obj.GetCampaign(Session["UserId"].ToString(), Session["email"].ToString());
                obj.getTotalRating(model);
                return View("CreateVideoTestimonial",model);
            }
            else
            {
                TempData["error"] = "Session expired";
                return RedirectToAction("index", "Home");
            }
        }
	}
}