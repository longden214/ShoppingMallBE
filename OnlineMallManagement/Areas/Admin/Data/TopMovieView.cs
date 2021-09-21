using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMallManagement.Areas.Admin.Data
{
    public class TopMovieView
    {
        public int MovieId { get; set; }
        public string MovieImage { get; set; }
        public string MovieName { get; set; }
        public int TotalTicket { get; set; }
    }
}