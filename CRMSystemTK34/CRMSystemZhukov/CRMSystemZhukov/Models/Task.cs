using System;

namespace CRMSystemZhukov.Models
{
    public class Task
    {
        public int Id { get; set; }
        public int DealId { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
    }
} 