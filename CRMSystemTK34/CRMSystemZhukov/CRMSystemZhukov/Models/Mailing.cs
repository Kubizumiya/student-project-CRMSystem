using System;

namespace CRMSystemZhukov.Models
{
    public class Mailing
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public DateTime Date { get; set; }
        public string Channel { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public string Statistics { get; set; }
    }
} 