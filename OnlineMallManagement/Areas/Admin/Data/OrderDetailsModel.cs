using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMallManagement.Areas.Admin.Data
{
    public class OrderDetailsModel
    {
        public int ShowId { get; set; }
        public string MoviveName { get; set; }
        public string ScreeningDate { get; set; }
        public string StartTime { get; set; }
        public string RoomName { get; set; }
        public string SeatName { get; set; }
        public double Price { get; set; }
        public int OrderId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}