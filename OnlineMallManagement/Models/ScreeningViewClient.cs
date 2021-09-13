using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMallManagement.Models
{
    public class ScreeningViewClient
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int MovieId { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
    }
}