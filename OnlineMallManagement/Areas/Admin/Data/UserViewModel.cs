using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMallManagement.Areas.Admin.Data
{
    public class UserViewModel
    {
        public int UserId { set; get; }
        public string DisplayName { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string RoleName { set; get; }
    }
}