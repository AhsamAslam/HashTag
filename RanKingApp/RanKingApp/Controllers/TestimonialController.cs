using FfmpegSharp;
using RanKingApp.Models;
using RankMonster.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace RankMonster.Controllers
{
    public class TestimonialController : Controller
    {
        //
        // GET: /Testimonial/
        public ActionResult Index()
        {
            return View("CreateTestimonial");
        }

        public ActionResult TestimonialPage(string id)
        {
            DAL obj = new DAL();
            Campaign cmp = obj.GetCampaign(id);
            string[] bonusIdLst = cmp.bonusId.Split(',');
            List<Bonuses> bLst = new List<Bonuses>();
            foreach (var item in bonusIdLst)
            {
                if (item != string.Empty)
                {
                    Bonuses b = obj.GetBonusById(item);
                    bLst.Add(b);
                }
            }
            TestimonialPageModel testimonialPageModel = new TestimonialPageModel();
            testimonialPageModel.campaignBonuses = bLst;
            testimonialPageModel.campaign = cmp;
            return View("index", testimonialPageModel);
        }
        public ActionResult UserResponsesVideo()
        {
            return View("UserResponsesVideo");
        }
        public ActionResult SaveTestimonial(string reviewer_fname, string reviewer_lname, string reviewer_email,
            string video_testimonial, string audio_testimonial, string campaign_name, string camp_id
            , string reviewer_comments, string rating, string userId)
        {

            DAL obj = new DAL();
            Testimonial testimonial = new Testimonial();
            testimonial.campaign_name = campaign_name;
            testimonial.camp_id = Convert.ToInt32(camp_id);
            testimonial.rating = Convert.ToInt32(rating);
            testimonial.reviewer_date = DateTime.Now.ToString();
            testimonial.reviewer_comments = reviewer_comments;
            testimonial.reviewer_email = reviewer_email;
            testimonial.video_testimonial = HttpUtility.HtmlEncode(video_testimonial);
            testimonial.reviewer_fname = reviewer_fname;
            testimonial.reviewer_lname = reviewer_lname;
            testimonial.UserId = Convert.ToInt32(userId);
            obj.SaveTestimonial(testimonial);
            //string newName = Guid.NewGuid().ToString() + ".webm";
            //string file_path = AppDomain.CurrentDomain.BaseDirectory + Path.Combine("uploads\\" + Session["UserId"] + "\\" + newName);
            //string newName_mp4 = Guid.NewGuid().ToString() + ".mp4";
            //string file_path_mp4 = AppDomain.CurrentDomain.BaseDirectory + Path.Combine("uploads\\" + Session["UserId"] + "\\" + newName_mp4);



            //if (!Directory.Exists(Server.MapPath("~/uploads/" + Session["UserId"])))
            //{
            //    Directory.CreateDirectory(Server.MapPath("~/uploads/" + Session["UserId"]));
            //}

            //video_testimonial = video_testimonial.Split(new string[] { "base64," }, StringSplitOptions.None)[1];
            //byte[] videoArray = Convert.FromBase64String(video_testimonial);
            //System.IO.File.WriteAllBytes(file_path, videoArray);


            //var path = AppDomain.CurrentDomain.BaseDirectory + "ffmpeg\\ffmpeg.exe";
            //FfmpegSharp.Ffmpeg ffmpeg = new FfmpegSharp.Ffmpeg(path);
            //OutputFile outFile = new OutputFile(file_path_mp4);

            //outFile.Video.Codec = "libx264";
            //outFile.Video.CustomArgs["crf"] = "25";
            //System.IO.File.Create(file_path_mp4);

            //ffmpeg.OverwriteOutput = true;
            //ffmpeg.Process(file_path, outFile);
            //ffmpeg.OnProgress += Ffmpeg_OnProgress;
            return View("ThanksYouPage");
        }


        public ActionResult GetTestimonial()
        {
            DAL obj = new DAL();
            List<Testimonial> testimonials = obj.GetTestimonialsByUserId(Convert.ToInt32(Session["UserId"].ToString()));
            return View("ViewTestimonial", testimonials);
        }
        private void Ffmpeg_OnProgress(object sender, ProgressEventArgs e)
        {

        }

        public ActionResult VideoTestimonialReport()
        {
            if (Session["UserId"] != null && Session["email"] != null)
            {
                DAL obj = new DAL();
                var model = obj.GetAllTestimonials(int.Parse(Session["UserId"].ToString()));
                return View("VideoTestimonialReport", model);
            }
            else
            {
                TempData["error"] = "Session expired";
                return RedirectToAction("index", "Home");
            }

        }
             public ActionResult removeTestimonial(int id)        {            DAL dAL = new DAL();            int x = dAL.DeleteTestimonial(id);            if (x > 1)            {                return new JsonResult() { Data = "Success" };            }            else            {                return new JsonResult() { Data = "Failed" };            }        }
    }
}