using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMallManagement.Models
{
    public class CartItem
    {
        public int SeatId { get; set; }
        public string SeatName { get; set; }
        public double SeatPrice { get; set; }
        public int ScreeningId { get; set; }
    }
}