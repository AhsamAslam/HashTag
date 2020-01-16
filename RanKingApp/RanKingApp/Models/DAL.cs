using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace RankMonster.Models
{
    public class DAL
    {
        public List<Campaign> GetCampaign(string userID, string email)
        {

            List<Campaign> lst = new List<Campaign>();
            object[] paramkw = new object[2];
            paramkw.SetValue(userID.Trim(), 0);
            paramkw.SetValue(email.Trim(), 1);

            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["connectionString"], "SP_GetCampaigns", paramkw);
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                lst.Add(HelperGetCampaign(item));
            }

            return lst;
        }
        public Campaign GetCampaign(string id)
        {
            object[] paramkw = new object[1];
            paramkw.SetValue(int.Parse(id.Trim()), 0);
            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["connectionString"], "SP_GetSingleCampaign", paramkw);
            Campaign cm = new Campaign();
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                cm = HelperGetCampaign(item);
            }
            return cm;
        }
        public Campaign HelperGetCampaign(DataRow item)
        {
            Campaign camp = new Campaign();
            camp.ID = item["ID"].ToString();
            camp.Name = item["Name"].ToString();
            camp.Title = item["Title"].ToString();
            camp.Desc = item["Desc"].ToString();
            camp.Image = item["Image"].ToString();
            camp.Status = item["Status"].ToString();
            camp.DateTime = item["DateTime"].ToString();
            camp.UserId = item["UserId"].ToString();
            camp.Email = item["Email"].ToString();
            camp.bonusId = item["bonusId"].ToString();
            camp.email_response = item["Email_Response"].ToString();
            camp.email_subject = item["Email_Subject"].ToString();
            return camp;
        }
        public Boolean UserExist(string email, int campId)
        {
            string query = string.Format("Select * from testimonials where reviewer_email=@email and camp_id=@campid");
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["connectionString"]))
            {
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@email", email.Trim());
                command.Parameters.AddWithValue("@campId", campId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

        }
        public void getTotalRating(List<Campaign> obj)
        {
            object[] paramkv = new object[1];

            foreach (var item in obj)
            {
                paramkv.SetValue(item.ID, 0);
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["connectionString"], "SP_GetRatings", paramkv);
                DataRow row = ds.Tables[0].Rows[0];
                string a = row[0].ToString();
                if (a != "")
                {
                    a = Math.Round(float.Parse(a), 0).ToString();
                    item.TotalRating = int.Parse(a);
                }
                //if (a !="" && a != null)
                //{

                //    Boolean n = a.Contains(".");
                //    if (n)
                //    {
                //        string num = a.Split('.')[1];
                //        if(int.Parse(num) != 5)
                //        {
                //            if (int.Parse(num) >= 50 && int.Parse(num) < 75)
                //            {
                //                a = a.Split('.')[0] + ".50";
                //                float b = float.Parse(a);
                //                item.TotalRating = b;
                //            }
                //            else if (int.Parse(num) >= 75 && int.Parse(num) < 100)
                //            {
                //                a = (int.Parse(a.Split('.')[0]) + 1).ToString();
                //                float b = float.Parse(a);
                //                item.TotalRating = b;
                //            }
                //            else if(int.Parse(num) >= 25 && int.Parse(num) < 50)
                //            {
                //                a = a.Split('.')[0] + ".50";
                //                float b = float.Parse(a);
                //                item.TotalRating = b;
                //            }
                //            else
                //            {
                //                float b = float.Parse(num);
                //                item.TotalRating = b;
                //            }
                //        }
                //        else
                //        {
                //            a = a.Split('.')[0];
                //            float b = float.Parse(a);
                //            item.TotalRating = b;

                //        }
                //    }
                //    else
                //    {
                //        float b = float.Parse(a);
                //        item.TotalRating = b;
                //    }


                //}

            }
        }
        public List<Testimonial> GetTestimonials(int campId)
        {
            object[] paramkv = new object[1];
            paramkv.SetValue(campId, 0);
            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["connectionString"], "SP_GetTestimonials", paramkv);
            Testimonial ts = new Testimonial();
            List<Testimonial> tsList = new List<Testimonial>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ts = HelperGetTestimonial(dr);
                tsList.Add(ts);
            }
            return tsList;
        }

        public List<Testimonial> GetTestimonialsByUserId(int userId)
        {
            object[] paramkv = new object[1];
            paramkv.SetValue(userId, 0);
            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["connectionString"], "GETAllTestimonials", paramkv);
            Testimonial ts = new Testimonial();
            List<Testimonial> tsList = new List<Testimonial>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ts = HelperGetTestimonial(dr);
                tsList.Add(ts);
            }
            return tsList;
        }
        public Testimonial HelperGetTestimonial(DataRow item)
        {
            Testimonial ts = new Testimonial();
            ts.camp_id = int.Parse(item["camp_id"].ToString());
            ts.t_id = int.Parse(item["t_id"].ToString());
            ts.reviewer_fname = item["reviewer_fname"].ToString();
            ts.reviewer_lname = item["reviewer_lname"].ToString();
            ts.reviewer_email = item["reviewer_email"].ToString();
            ts.reviewer_date = item["reviewer_date"].ToString();
            ts.reviewerImage = item["reviewerImage"].ToString();
            float.TryParse(item["rating"].ToString(), out float num);
            ts.rating = num;
            ts.reviewer_comments = item["reviewer_comments"].ToString();
            ts.video_testimonial = item["video_testimonial"].ToString();
            ts.youtube_url = item["youtube_url"].ToString();
            if (item.Table.Columns["CampName"] != null)
            {
                ts.campaign_name = item["CampName"].ToString();
            }

            return ts;
        }
        public List<Campaign> GetTopCampaigns(string userID, string email)
        {
            object[] Paramvk = new object[2];
            List<Campaign> lst = new List<Campaign>();
            Paramvk.SetValue(userID, 0);
            Paramvk.SetValue(email, 1);
            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["connectionString"], "SP_GetTopCampaigns", Paramvk);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(HelperGetCampaign(dr));
            }
            return lst;
        }
        public List<Testimonial> GetTopTestimonials(int UserId)
        {
            object[] paramkv = new object[1];
            paramkv.SetValue(UserId, 0);
            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["connectionString"], "SP_GetTopTestimonials", paramkv);
            Testimonial ts = new Testimonial();
            List<Testimonial> tsList = new List<Testimonial>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ts = HelperGetTestimonial(dr);
                tsList.Add(ts);
            }
            return tsList;
        }
        public int getTotalCampaigns(int UserId)
        {
            int count = 0;
            object[] paramkv = new object[1];
            paramkv.SetValue(UserId, 0);
            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["connectionString"].ToString(), "SP_TotalCampaigns", paramkv);
            DataRow dr = ds.Tables[0].Rows[0];
            if (dr["count"].ToString() != "")
            {
                count = int.Parse(dr["count"].ToString());
            }
            return count;
        }
        public int getTotalLinkGenerated(int UserId)
        {
            int count = 0;
            object[] paramkv = new object[1];
            paramkv.SetValue(UserId, 0);
            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["connectionString"].ToString(), "SP_TotalLinkGenerated", paramkv);
            DataRow dr = ds.Tables[0].Rows[0];
            if (dr["count"].ToString() != "")
            {
                count = int.Parse(dr["count"].ToString());
            }
            return count;
        }
        public int getTotalVideoMonials(int UserId)
        {
            int count = 0;
            object[] paramkv = new object[1];
            paramkv.SetValue(UserId, 0);
            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["connectionString"].ToString(), "SP_TotalVideomonialRecieved", paramkv);
            DataRow dr = ds.Tables[0].Rows[0];
            if (dr["count"].ToString() != "")
            {
                count = int.Parse(dr["count"].ToString());
            }
            return count;
        }
        public double getTotalAverageRating(int UserId)
        {
            double count = 0.0;
            object[] paramkv = new object[1];
            paramkv.SetValue(UserId, 0);
            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["connectionString"].ToString(), "SP_TotalAverageRating", paramkv);
            DataRow dr = ds.Tables[0].Rows[0];
            if (dr["count"].ToString() != "")
            {
                count = double.Parse(dr["count"].ToString());
            }
            return count;
        }
        public List<Testimonial> GetAllTestimonials(int UserId)
        {
            object[] paramkv = new object[1];
            paramkv.SetValue(UserId, 0);
            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["connectionString"], "SP_GetAllTestimonials", paramkv);
            Testimonial ts = new Testimonial();
            List<Testimonial> tsList = new List<Testimonial>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ts = HelperGetTestimonial(dr);
                tsList.Add(ts);
            }
            return tsList;
        }
        public int SaveBonus(Bonuses bonus)
        {
            if (bonus.b_id > 0)
            {

                object[] param = new object[6];
                param.SetValue(bonus.b_id, 0);
                param.SetValue(bonus.b_name.Trim(), 1);
                param.SetValue(bonus.b_description.Trim(), 2);
                param.SetValue(bonus.b_access_url.Trim(), 3);
                param.SetValue(bonus.b_image.Trim(), 4);
                param.SetValue(bonus.userId, 5);

                int id = SqlHelper.ExecuteNonQuery(ConfigurationManager.AppSettings["connectionString"].ToString(), "SP_UpdateBonus", param);
                return Convert.ToInt32(id);
            }
            else
            {
                object[] param = new object[5];
                param.SetValue(bonus.b_name.Trim(), 0);
                param.SetValue(bonus.b_description.Trim(), 1);
                param.SetValue(bonus.b_access_url.Trim(), 2);
                param.SetValue(bonus.b_image.Trim(), 3);
                param.SetValue(bonus.userId, 4);
                Object id = SqlHelper.ExecuteScalar(ConfigurationManager.AppSettings["connectionString"].ToString(), "SP_SaveBonus", param);
                return Convert.ToInt32(id);
            }

        }
        public void SaveTestimonial(Testimonial testimonial)
        {
            object[] param = new object[11];
            param.SetValue(testimonial.camp_id, 0);
            param.SetValue(testimonial.reviewer_fname.Trim(), 1);
            param.SetValue(testimonial.reviewer_lname.Trim(), 2);
            param.SetValue(testimonial.reviewer_email, 3);
            param.SetValue(testimonial.reviewer_date, 4);
            param.SetValue(testimonial.reviewer_comments.Trim(), 5);
            param.SetValue(testimonial.rating, 6);
            param.SetValue(testimonial.youtube_url, 7);
            param.SetValue(testimonial.video_testimonial, 8);
            param.SetValue(testimonial.reviewerImage, 9);
            param.SetValue(testimonial.UserId, 10);
            SqlHelper.ExecuteNonQuery(ConfigurationManager.AppSettings["connectionString"].ToString(), "SP_SaveTestimonial", param);
        }
        public List<Bonuses> GetAllBonuses(String UserId)
        {
            string con = ConfigurationManager.AppSettings["connectionString"].ToString();
            object[] param = new object[1];
            param.SetValue(UserId, 0);
            DataSet ds = SqlHelper.ExecuteDataset(con, "GETAllBonuses", param);
            List<Bonuses> obj = new List<Bonuses>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Bonuses b = HelperGetBonus(dr);
                obj.Add(b);
            }
            return obj;
        }
        private Bonuses HelperGetBonus(DataRow dr)
        {
            Bonuses obj = new Bonuses();
            obj.b_id = Convert.ToInt32(dr["b_id"].ToString());
            obj.b_name = dr["b_name"].ToString();
            obj.b_description = dr["b_description"].ToString();
            obj.b_image = dr["b_image"].ToString();
            obj.b_access_url = dr["b_access_url"].ToString();
            return obj;
        }
        public Bonuses GetSingleBonus(string b_name, string UserId)
        {
            object[] param = new object[2];
            param.SetValue(b_name, 0);
            param.SetValue(UserId, 1);
            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["connectionString"].ToString(), "GETSingleBonus", param);
            if (ds.Tables[0].Rows.Count != 0)
            {
                Bonuses b = HelperGetBonus(ds.Tables[0].Rows[0]);
                return b;
            }
            else
            {
                Bonuses b = new Bonuses();
                return b;
            }

        }
        public Bonuses GetSingleBonusById(int b_id, string UserId)
        {
            object[] param = new object[2];
            param.SetValue(b_id, 0);
            param.SetValue(Convert.ToInt32(UserId), 1);
            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["connectionString"].ToString(), "GETSingleBonusById", param);
            if (ds.Tables[0].Rows.Count != 0)
            {
                Bonuses b = HelperGetBonus(ds.Tables[0].Rows[0]);
                return b;
            }
            else
            {
                Bonuses b = new Bonuses();
                return b;
            }

        }
        public int deleteBonus(string name, string userId)
        {
            object[] param = new object[2];
            param.SetValue(name.Trim(), 0);
            param.SetValue(userId, 1);

            int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.AppSettings["connectionString"].ToString(), "SP_DeleteBonus", param);
            return x;
        }
        public Bonuses GetBonusById(string id)
        {

            string con = ConfigurationManager.AppSettings["connectionString"].ToString();
            object[] param = new object[1];
            param.SetValue(id, 0);
            DataSet ds = SqlHelper.ExecuteDataset(con, "GETBonusById", param);
            Bonuses b = HelperGetBonus(ds.Tables[0].Rows[0]);
            return b;
        }
        public void SendEmail(string receiver, string usermessage, string email_subject)
        {
            var fromAddress = new MailAddress("vidmonialbravinn@gmail.com", "Videomonial");
            var toAddress = new MailAddress(receiver);
            const string fromPassword = "k{87?5Jf%MQ8/,Dk";
            string subject = email_subject;
            string body = usermessage + "\n\n it is a @non reply email please dont reply reponse will not be given!";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        public List<string> getDashBoardData(String User, string email)
        {
            object[] param = new object[2];
            param.SetValue(User, 0);
            param.SetValue(email.Trim(), 1);
            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["connectionString"], "SP_getDashBoardData", param);
            List<string> data = new List<string>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                data.Add(dr["campaigns"].ToString());
                data.Add(dr["rating"].ToString());
                data.Add(dr["testimonials"].ToString());
                data.Add(dr["Links"].ToString());

            }
            return data;
        }
        public int deleteCampaign(int userID, int campId)
        {
            object[] param = new object[2];
            param.SetValue(userID, 0);
            param.SetValue(campId, 1);
            int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.AppSettings["connectionString"].ToString(), "SP_DeleteCampaign", param);

            return (x);
        }
        public int DeleteTestimonial(int id)
        {
            object[] param = new object[1];
            param.SetValue(id, 0);

            int x = SqlHelper.ExecuteNonQuery(ConfigurationManager.AppSettings["connectionString"].ToString(), "SP_DeleteTestimonial", param);
            return x;
        }
    }
}
