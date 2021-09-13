using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMallManagement.Models
{
    public class FeedbackViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string FBDate { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}