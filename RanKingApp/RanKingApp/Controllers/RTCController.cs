using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RankMonster.Controllers
{
    public class RTCController : Controller
    {
        //
        // GET: /RTC/
        public ActionResult Index()
        {
            return View("RTC");
        }

        // ---/RecordRTC/PostRecordedAudioVideo
        [HttpPost]
        public ActionResult PostRecordedAudioVideo()
        {
            foreach (string upload in Request.Files)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "uploads/";
                var file = Request.Files[upload];
                if (file == null) continue;

                file.SaveAs(Path.Combine(path, Request.Form[0]));
            }
            return Json(Request.Form[0]);
        }

        // ---/RecordRTC/DeleteFile
        [HttpPost]
        public ActionResult DeleteFile()
        {
            var fileUrl = AppDomain.CurrentDomain.BaseDirectory + "uploads/" + Request.Form["delete-file"] + ".webm";
            new FileInfo(fileUrl).Delete();
            return Json(true);
        }
	}
}