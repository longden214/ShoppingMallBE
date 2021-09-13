using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    public class AreaController : Controller
    {
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();

        [Authorize(Roles = "Admin,User,Customer")]
        public ActionResult Index()
        {
            var data = dbContext.Areas.ToList();
            return View(data);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Area a)
        {
            var area = dbContext.Areas.Where(x => x.AreaName.Equals(a.AreaName)).Count();
            try
            {
                if (area == 0)
                {
                    a.CreatedDate = DateTime.Now;
                    a.ModifiedDate = DateTime.Now;
                    var result = dbContext.Areas.Add(a);
                    dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("AreaName", "AreaName already exists!");
                }

            }
            catch
            {
                return View();
            }
            return View();
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Edit(int id)
        {
            var area = dbContext.Areas.Single(x => x.AreaId == id);
            return View(area);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Area a)
        {
            var areas = dbContext.Areas.Where(x => x.AreaName.Equals(a.AreaName)).Count();
            try
            {
                if (areas == 0)
                {
                    Area area = dbContext.Areas.Single(x => x.AreaId == id);
                    area.AreaName = a.AreaName;
                    area.ModifiedDate = DateTime.Now;
                    dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("AreaName", "AreaName already exists!");
                }
            }
            catch
            {
                return View(a);
            }
            return View(a);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Delete(int id)
        {
            var area = dbContext.Areas.Single(x => x.AreaId == id);
            return View(area);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Area a)
        {
            try
            {
                var result = dbContext.Areas.Single(x => x.AreaId == id);
                dbContext.Areas.Remove(result);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(a);
            }
        }
    }
}