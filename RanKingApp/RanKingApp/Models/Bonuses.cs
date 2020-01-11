using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RankMonster.Models
{
    public class Bonuses
    {
        public int b_id { get; set; }
        public string b_name { get; set; }
        public string b_description { get; set; }
        public string b_access_url { get; set; }
        public string b_image { get; set; }
        public int userId { get; set; }
       
    }
}