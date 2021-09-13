using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMallManagement.Areas.Admin.Data
{
    public class ScreeningViewModel
    {
        public int ScreeningId { set; get; }
        public string MovieImg { set; get; }
        public string MovieName { set; get; }
        public string CinemaHall { set; get; }
        public string Date { set; get; }
        public string Time { set; get; }
        public bool Status { set; get; }
    }
}