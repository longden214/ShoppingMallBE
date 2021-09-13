using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Controllers
{
    public class HomeController : Controller
    {
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();
        public ActionResult Index()
        {
            ViewBag.Banners = dbContext.Banners.OrderBy(x => x.OrderBy).Where(x => x.page.Equals("Home")).ToList();
            ViewBag.Events = dbContext.Events.Where(x => x.Status == true).Take(2);
            ViewBag.Blogs = dbContext.Blogs.Where(x => x.Status == true).Take(3);

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Banner = dbContext.Banners.Find(18);
            ViewBag.Phone = dbContext.Configs.Find(10).value;
            ViewBag.Email = dbContext.Configs.Find(11).value;
            ViewBag.Address = dbContext.Configs.Find(8).value;
            ViewBag.Open = dbContext.Configs.Find(9).value;
            ViewBag.GPS = dbContext.Configs.Find(7).value;

            return View();
        }

        public ActionResult Directions()
        {
            ViewBag.Banner = dbContext.Banners.Find(19);
            ViewBag.GPS = dbContext.Configs.Find(7).value;

            return View();
        }

        public ActionResult GalleryMasonry()
        {
            ViewBag.Banner = dbContext.Banners.Find(20);
            ViewBag.Gallery = dbContext.galleries.Where(x => x.status == true).ToList();

            return View();
        }

        public ActionResult SaleSingle(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Sales");
            }

            ViewBag.Banner = dbContext.Banners.Find(29);
            var sale = dbContext.Sales.Find(id);
            ViewBag.Sales = sale;
            ViewBag.Product = dbContext.Shop_Product.Where(x => x.Shop_ShoId == sale.Shop_ShoId).ToList();
            ViewBag.Shop = dbContext.Shops.Where(x => x.ShoId == sale.Shop_ShoId).FirstOrDefault();

            return View();
        }

        public ActionResult Sales()
        {

            ViewBag.Banner = dbContext.Banners.Find(21);
            ViewBag.Sales = dbContext.Sales.Where(x => x.Status == true).ToList();

            return View();
        }

        public ActionResult SearchResults()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult SEOWeb()
        {
            ViewBag.favicon = dbContext.Configs.Find(15).value;
            ViewBag.meta_keyword = dbContext.Configs.Find(13).value;
            ViewBag.meta_description = dbContext.Configs.Find(14).value;

            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult TopHeader()
        {
            ViewBag.Configs = dbContext.Configs.ToList();
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult BottomFooter()
        {

            ViewBag.Logo = dbContext.Configs.Find(16).value;
            ViewBag.Description = dbContext.Configs.Find(14).value;

            return PartialView();
        }
    }
}