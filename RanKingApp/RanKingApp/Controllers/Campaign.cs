
using Microsoft.ApplicationBlocks.Data;
using RankMonster.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace RankMonster.Controllers
{
    public class CampaignController : Controller
    {

        public ActionResult CreateCampaign()
        {
            if (Session["UserId"] != null && Session["email"] != null)
            {
                Message message = new Message();
                message.title = "Create a New Campaign";
                message.buttonName = "Save Campaign";
                ViewData["data"] = message;
                DAL obj = new DAL();
                List<Bonuses> b = obj.GetAllBonuses(Session["UserId"].ToString());
                Session["Bonuses"] = b;
                return View(b);
            }
            else
            {
                TempData["error"] = "Session expired";
                return RedirectToAction("index", "Home");
            }

        }

        [HttpPost]
        public ActionResult Save(string CampName, string txtTitle, string txtDesc, string[] bonusId, string email_response, HttpPostedFileBase Image, string email_subject)
        {
            try
            {
                if (!Directory.Exists(Server.MapPath("~/User_Campaigns/" + Session["UserId"])))
                {
                    Directory.CreateDirectory(Server.MapPath("~/User_Campaigns/" + Session["UserId"]));
                }
                string name = "";
                object[] paramkw = new object[10];
                if (Image != null)
                {
                    name = Guid.NewGuid().ToString() + "." + Image.FileName.Split('.')[1];
                    name = Path.Combine("User_Campaigns/" + Session["UserId"] + "/" + name);
                    paramkw.SetValue(CampName.Trim(), 0);
                    paramkw.SetValue(txtTitle.Trim(), 1);
                    paramkw.SetValue(txtDesc.Trim(), 2);
                    paramkw.SetValue(name, 3);
                    paramkw.SetValue(Session["UserId"], 4);
                    paramkw.SetValue(Session["email"], 5);
                    paramkw.SetValue(DateTime.Now.ToString().Trim(), 6);
                    string bonuses = "";
                    for (int x = 0; x < bonusId.Length; x++)
                    {
                        bonuses += bonusId[x] + ",";
                    }
                    paramkw.SetValue(bonuses, 7);
                    paramkw.SetValue(email_response, 8);
                    paramkw.SetValue(email_subject, 9);
                }
                else
                {
                    TempData["CampaignSave"] = "NoImage";
                }

                Object id = SqlHelper.ExecuteScalar(ConfigurationManager.AppSettings["connectionString"], "SP_SaveCampaign", paramkw);
                if (id.ToString() != "")
                {
                    var folder = Server.MapPath("~/" + name);

                    Image.SaveAs(folder);

                    TempData["CampaignSave"] = "success";
                }
                else
                {
                    TempData["CampaignSave"] = "failed";
                }

                return RedirectToAction("CreateCampaign");
            }
            catch (Exception ex)
            {
                return RedirectToAction("CreateCampaign");
            }
        }

        public ActionResult ViewCampaign()
        {

            DAL obj = new DAL();
            var model = obj.GetCampaign(Session["UserId"].ToString(), Session["email"].ToString());
            return View(model);
        }
        public Boolean GenerateCampaign(string id)
        {
            Boolean result = false;
            if (int.Parse(id) > 0)
            {
                string query = string.Format("Update Campaign set Generatelinks='{0}' where id='{1}'", true, id);
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connectionString"]))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    int x = cmd.ExecuteNonQuery();
                    if (x > 0)
                    {
                        result = true;
                        string lnk = string.Format("~/Campaign/GenerateCampaign/{0}'", id);

                    }
                    else
                    {
                        result = false;
                    }
                    conn.Close();
                }
            }
            return result;
        }
        public ActionResult Edit(string id)
        {
            DAL obj = new DAL();
            Campaign cmp = obj.GetCampaign(id);
            List<Bonuses> b = obj.GetAllBonuses(Session["UserId"].ToString());
            Session["Bonuses"] = b;
            ViewData["campaign"] = cmp;
            return View("UpdateCampaign");
        }
        public ActionResult UpdateCampaign(string ID, string CampName, string txtTitle, string[] bonusId, string email_response, string txtDesc, HttpPostedFileBase Image, string email_subject,string user_file)
        {
            try
            {
                if (!Directory.Exists(Server.MapPath("~/User_Campaigns/" + Session["UserId"])))
                {
                    Directory.CreateDirectory(Server.MapPath("~/User_Campaigns/" + Session["UserId"]));
                }
                string name = "";
                object[] paramkw = new object[11];
                if (Image != null)
                {
                    if(System.IO.File.Exists(Server.MapPath("~/" + user_file)))
                    {
                        System.IO.File.Delete(Server.MapPath("~/" + user_file));
                    }
                    name = Guid.NewGuid().ToString() + "." + Image.FileName.Split('.')[1];
                    name = Path.Combine("User_Campaigns/" + Session["UserId"] + "/" + name);
                    
                }
                else
                {
                    name = user_file;
                    TempData["CampaignSave"] = "NoImage";
                }
                paramkw.SetValue(CampName.Trim(), 0);
                paramkw.SetValue(txtTitle.Trim(), 1);
                paramkw.SetValue(txtDesc.Trim(), 2);
                paramkw.SetValue(name, 3);
                paramkw.SetValue(Session["UserId"], 4);
                paramkw.SetValue(Session["email"], 5);
                paramkw.SetValue(DateTime.Now.ToString().Trim(), 6);
                string bonuses = "";
                for (int x = 0; x < bonusId.Length; x++)
                {
                    bonuses += bonusId[x] + ",";
                }
                paramkw.SetValue(bonuses, 7);
                paramkw.SetValue(email_response, 8);
                paramkw.SetValue(email_subject, 9);
                paramkw.SetValue(ID, 10);
                Object id = SqlHelper.ExecuteScalar(ConfigurationManager.AppSettings["connectionString"], "SP_UpdateCampaign", paramkw);

                if(Image != null)
                {
                    var folder = Server.MapPath("~/" + name);

                    Image.SaveAs(folder);

                }
                TempData["CampaignSave"] = "success";
                
                return RedirectToAction("ViewCampaign");
            }
            catch (Exception ex)
            {
                return RedirectToAction("CreateCampaign");
            }
        }
        public ActionResult RemoveCampaign(int id)
        {
            DAL obj = new DAL();
            int x = obj.deleteCampaign(int.Parse(Session["UserId"].ToString()), id);
            return RedirectToAction("ViewCampaign");
        }


    }
}
