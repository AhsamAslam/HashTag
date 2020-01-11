using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RankMonster.Models
{
    public class Campaign
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

        public string Desc { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public string DateTime { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public int TotalRating { get; set; }
        public string bonusId { get; set; }
        public string email_response { get; set; }
        public string email_subject { get; set; }

    }
}