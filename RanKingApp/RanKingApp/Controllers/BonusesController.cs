using RankMonster.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RanKingApp.Controllers
{
    public class BonusesController : Controller
    {
        // GET: Bonuses
        
        public ActionResult Bonuses()
        {
            DAL dal = new DAL();
            List<Bonuses> obj = dal.GetAllBonuses(Session["UserId"].ToString());
            return View("ViewBonus", obj);
        }
        public ActionResult AddBonus()
        {
            return View("AddBonus");
        }
        public ActionResult SaveBonus(Bonuses bonus, HttpPostedFileBase file)
        {
            if (!Directory.Exists(Server.MapPath("~/Bonuses/" + Session["UserId"])))
            {
                Directory.CreateDirectory(Server.MapPath("~/Bonuses/" + Session["UserId"]));
            }
            DAL obj = new DAL();
            using (MD5 md5Hash = MD5.Create())
            {
                string file_path = null;
                if (file != null)
                {
                    string ext = file.FileName.Split('.')[1].ToString();
                    string Filename = bonus.b_name;
                    string newName = Guid.NewGuid().ToString() + "." + ext;
                    file_path = Path.Combine("Bonuses/" + Session["UserId"] + "/" + newName);
                    bonus.b_image = file_path;
                    file.SaveAs(Server.MapPath("~/" + file_path));
                }
                else
                {

                    file_path = bonus.b_image;
                }
                if (bonus.b_name != null && bonus.b_access_url != null && bonus.b_description != null)
                {
                    bonus.userId = Convert.ToInt32(Session["UserId"].ToString());

                    int x = obj.SaveBonus(bonus);
                    if (x > 0)
                    {
                        bonus.b_id = x;
                        TempData["UserBonus"] = "success";
                    }
                    else
                    {
                        TempData["UserBonus"] = "failed";
                    }

                }
                else
                {
                    TempData["UserBonus"] = "failed";
                }

            }
            return RedirectToAction("AddBonus");
        }
        public ActionResult EditBonus(string name)
        {
            DAL dal = new DAL();
            Bonuses bonus = dal.GetSingleBonus(name, Session["UserId"].ToString());
            return View("EditBonus", bonus);
        }

        public ActionResult UpdateBonus(Bonuses bonus, HttpPostedFileBase file)
        {
            if (!Directory.Exists(Server.MapPath("~/Bonuses/" + Session["UserId"])))
            {
                Directory.CreateDirectory(Server.MapPath("~/Bonuses/" + Session["UserId"]));
            }
            using (MD5 md5Hash = MD5.Create())
            {
                if (file != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("~/"+ bonus.b_image)))
                    {
                        System.IO.File.Delete(Server.MapPath("~/" + bonus.b_image));
                    }
                    string ext = file.FileName.Split('.')[1].ToString();
                    string Filename = bonus.b_name;
                    string newName = Guid.NewGuid().ToString() + "." + file.FileName.Split('.')[1];
                    string file_path = Path.Combine("Bonuses/" + Session["UserId"] + "/" + newName);
                    bonus.b_image = file_path;
                    file.SaveAs(Server.MapPath("~/" + file_path));

                }
                bonus.userId = Convert.ToInt32(Session["UserId"].ToString());
                DAL obj = new DAL();
                int x = obj.SaveBonus(bonus);
                if (x > 0)
                {
                    TempData["UpdateBonus"] = "success";
                }
                else
                {
                    TempData["UpdateBonus"] = "failed";
                }
                return RedirectToAction("Bonuses");
            }
        }
        public ActionResult DeleteBonus(string id, string image)
        {
            DAL obj = new DAL();
            int x = obj.deleteBonus(id, Session["UserId"].ToString());
            if (x > 0)
            {

                if (System.IO.File.Exists(Server.MapPath("~/" + image)))
                {
                    System.IO.File.Delete(Server.MapPath("~/" + image));

                }
                return new JsonResult() { Data = "success" };
            }
            else
            {
                return new JsonResult() { Data = "failed" };
            }
        }
        public string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}