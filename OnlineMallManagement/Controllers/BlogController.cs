using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Controllers
{
    public class BlogController : Controller
    {
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();
        public ActionResult BlogClassic(string SearchString, int? page,int? cate)
        {
            ViewBag.Banner = dbContext.Banners.Find(16);
            ViewBag.Category = dbContext.Category_blog.ToList();

            var ressult = (from s in dbContext.Blogs where s.Status == true select s).ToList();

            if (!string.IsNullOrEmpty(SearchString))
            {
                ressult = dbContext.Blogs.Where(s => s.Title.Contains(SearchString) && s.Status == true).ToList();
            }

            if (cate != null)
            {
                ressult = dbContext.Blogs.Where(s => s.Category_blog_id == cate && s.Status == true).ToList();
            }

            page = page ?? 1;

            int limit = 5;
            int start = (int)(page - 1) * limit;
            int totalShop = ressult.Count();

            ViewBag.totalShop = totalShop;
            ViewBag.pageCurrent = page;

            float numberPage = (float)totalShop / limit;

            ViewBag.numberPage = (int)Math.Ceiling(numberPage);

            var dataShop = ressult.OrderByDescending(s => s.BlogId).Skip(start).Take(limit);

            return View(dataShop);
        }

        public ActionResult BlogPost(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BlogClassic");
            }

            ViewBag.Banner = dbContext.Banners.Find(4);
            ViewBag.Category = dbContext.Category_blog.ToList();

            ViewBag.BlogSingle = dbContext.Blogs.Find(id);

            return View();
        }

       
    }
}