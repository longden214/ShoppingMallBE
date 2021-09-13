using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMallManagement.Areas.Admin.Data
{
    public class SeatViewModel
    {
        public int SeatId { set; get; }
        public string SeatName { set; get; }
        public double SeatPrice { set; get; }
        public string SeatHall { set; get; }
    }
}