using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RankMonster.Models
{
    public class Testimonial
    {
        public int t_id { get; set; }
        public int camp_id { get; set; }
        public string reviewer_fname { get; set; }
        public string reviewer_lname { get; set; }
        public string reviewer_email { get; set; }
        public string reviewer_date { get; set; }
        public string reviewer_comments { get; set; }
        public float rating { get; set; }
        public string youtube_url { get; set; }
        public string video_testimonial { get; set; }
        public string video_url { get; set; }
        public string reviewerImage { get; set; }
        public int UserId { get; set; }
        public string campaign_name { get; set; }
        public string CampImage { get; set; }

    }
}