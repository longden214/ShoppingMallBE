using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    public class GalleryController : Controller
    {
        private DBOnlineMallEntities db;

        public GalleryController()
        {
            db = new DBOnlineMallEntities();
        }

        [Authorize(Roles = "Admin,User,Customer")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List(int? page, int? pageSize)
        {

            var gallaryList = db.galleries.ToList();

            var _pageSize = pageSize ?? 8;
            var pageIndex = page ?? 1;
            var totalPage = gallaryList.Count();
            var numberPage = Math.Ceiling((double)totalPage / _pageSize);

            var data = gallaryList.Skip((pageIndex - 1) * _pageSize).Take(_pageSize);
            return Json(new
            {
                proList = data,
                TotalItems = totalPage,
                CurrentPage = pageIndex,
                NumberPage = numberPage,
                PageSize = _pageSize
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteGallery(int id)
        {
            var result = false;
            try
            {
                var proItem = db.galleries.Find(id);
                db.galleries.Remove(proItem);
                db.SaveChanges();

                result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult EditStatus(int id)
        {
            bool result = false;
            try
            {
                var pro1 = db.galleries.Find(id);
                var pro2 = db.galleries.Find(id);
                if (pro1.status == true)
                {
                    pro2.status = false;
                }
                else
                {
                    pro2.status = true;
                }

                db.Entry(pro2).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Add(gallery g)
        {


            g.createdDate = DateTime.Now;
            g.Modifidate = DateTime.Now;
            db.galleries.Add(g);
            db.SaveChanges();
            return Json(db.SaveChanges(), JsonRequestBehavior.AllowGet);
        }
    }
}