using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    public class BannerController : Controller
    {
        // GET: Admin/Banner
        private DBOnlineMallEntities db;

        public BannerController()
        {
            db = new DBOnlineMallEntities();
        }

        [Authorize(Roles = "Admin,User,Customer")]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult List()
        {
            var check = true;
            string[] roleNames = Roles.GetRolesForUser();
            if (roleNames[0].Contains("Customer") && roleNames.Length == 1)
            {
                check = false;
            }

            return Json(new { data = db.Banners.AsEnumerable(),checkRole = check }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyPage(string page)
        {
            var Banner = db.Banners.FirstOrDefault(x => x.page.Contains(page));
            return Json(Banner, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getbyID(int Id)
        {
            var Banner = db.Banners.FirstOrDefault(x => x.Id == Id);
            return Json(Banner, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin,User")]
        public JsonResult Update(Banner b)
        {
            try
            {
                Banner banner = db.Banners.FirstOrDefault(x => x.Id == b.Id);
                var cbanner = db.Banners.Where(x => x.OrderBy.Equals(b.OrderBy) && x.page.Equals(b.page)).Count();

                if ((cbanner == 0 && banner.OrderBy != b.OrderBy) || (cbanner == 1 && banner.OrderBy == b.OrderBy))
                {
                    if (banner != null)
                    {
                        banner.Image = b.Image;
                        banner.link = b.link;
                        banner.description = b.description;
                        banner.OrderBy = b.OrderBy;
                        banner.Sratus = b.Sratus;
                        banner.page = b.page;
                        banner.ModifiedDate = DateTime.Now;

                        return Json(db.SaveChanges(), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {

                return null;
            }
            return null;

        }
    }
}