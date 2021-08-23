using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Controllers
{
    public class ServiceController : Controller
    {
        // GET: Service
        public ActionResult Entertainment()
        {
            return View();
        }


        public ActionResult Food()
        {
            return View();
        }

        public ActionResult Parking()
        {
            return View();
        }

        public ActionResult RestaurantSingle()
        {
            return View();
        }

        public ActionResult Shops()
        {
            return View();
        }

        public ActionResult ShopSingle()
        {
            return View();
        }
    }
}