using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Controllers
{
    public class ServiceController : Controller
    {
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();
        // GET: Service
        public ActionResult Entertainment()
        {
            ViewBag.Banner = dbContext.Banners.Find(10);

            var data = (from sv in dbContext.Services
                        join a in dbContext.Areas on sv.Service_area equals a.AreaId
                        join s in dbContext.Shops on sv.ServiceId equals s.ServiceId
                        where a.AreaId == 2 && s.Status == true
                        select s).ToList();

            ViewBag.Entertainment = data;

            return View();
        }


        public ActionResult Food()
        {
            ViewBag.Banner = dbContext.Banners.Find(12);
            ViewBag.FoodFilter = dbContext.Services.Where(x => x.Service_area == 1).ToList();

            var food = (from sv in dbContext.Services
                        join a in dbContext.Areas on sv.Service_area equals a.AreaId
                        join s in dbContext.Shops on sv.ServiceId equals s.ServiceId
                        where a.AreaId == 1 && s.Status == true
                        select s).ToList();

            ViewBag.Foods = food;

            return View();
        }

        public ActionResult Parking()
        {
            ViewBag.Banner = dbContext.Banners.Find(13);

            var data = (from sv in dbContext.Services
                        join a in dbContext.Areas on sv.Service_area equals a.AreaId
                        join s in dbContext.Shops on sv.ServiceId equals s.ServiceId
                        where a.AreaId == 3 && s.Status == true
                        select s).ToList();

            ViewBag.Parking = data;


            return View();
        }

        public ActionResult RestaurantSingle(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Food");
            }

            ViewBag.Banner = dbContext.Banners.Find(25);

            var item = dbContext.Shops.Find(id);
            ViewBag.Food = item;

            ViewBag.Product = dbContext.Shop_Product.Where(x => x.Shop_ShoId == id && x.Status == true).ToList();

            return View();
        }

        public ActionResult Shops()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            ViewBag.Banner = dbContext.Banners.Find(11);
            ViewBag.ShopFilter = dbContext.Services.Where(x => x.Service_area == 9).ToList();

            var shop = (from sv in dbContext.Services
                        join a in dbContext.Areas on sv.Service_area equals a.AreaId
                        join s in dbContext.Shops on sv.ServiceId equals s.ServiceId
                        where a.AreaId == 9 && s.Status == true
                        select s).ToList();

            ViewBag.Shops = shop;

            return View();
        }

        public ActionResult ShopSingle(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Shops");
            }

            ViewBag.Banner = dbContext.Banners.Find(24);

            var item = dbContext.Shops.Find(id);
            ViewBag.Shop = item;
            ViewBag.Product = dbContext.Shop_Product.Where( x => x.Shop_ShoId == id && x.Status == true).ToList();

            var shopOther = (from sv in dbContext.Services
                        join a in dbContext.Areas on sv.Service_area equals a.AreaId
                        join s in dbContext.Shops on sv.ServiceId equals s.ServiceId
                        where a.AreaId == 9 && s.Status == true && s.ShoId != id
                        select s).Take(4);

            ViewBag.Order = shopOther;
            return View();
        }
    }
}