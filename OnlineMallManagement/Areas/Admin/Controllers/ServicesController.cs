using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    public class ServicesController : Controller
    {
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();

        [Authorize(Roles = "Admin,User,Customer")]
        public ActionResult Index()
        {
            var ressult = from s in dbContext.Services orderby s.Area.AreaName descending select s;
            return View(ressult);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Create()
        {
            ViewBag.Service_area = new SelectList(dbContext.Areas, "AreaId", "AreaName");
            return View();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Service s)
        {
            var service = dbContext.Services.Where(x => x.ServiceName.Equals(s.ServiceName)).Count();
            try
            {
                ViewBag.Service_area = new SelectList(dbContext.Areas, "AreaId", "AreaName");
                if (ModelState.IsValid)
                {
                    if (service == 0)
                    {
                        s.CreatedDate = DateTime.Now;
                        s.ModifiedDate = DateTime.Now;
                        var result = dbContext.Services.Add(s);
                        dbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("ServiceName", "ServiceName already exists!");
                    }
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
            var result = dbContext.Services.Single(x => x.ServiceId == id);
            ViewBag.area_id = new SelectList(dbContext.Areas.ToList(), "AreaId", "AreaName", result.Service_area);

            return View(result);

        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Service s)
        {
            var services = dbContext.Services.Where(x => x.ServiceName.Equals(s.ServiceName)).Count();
            var serviceItem = dbContext.Services.Find(id);
            try
            {
                ViewBag.Service_area = new SelectList(dbContext.Areas.ToList(), "AreaId", "AreaName", s.Service_area);
                if (ModelState.IsValid)
                {
                    if (serviceItem.ServiceName == s.ServiceName)
                    {

                        Service service = dbContext.Services.Single(x => x.ServiceId == id);
                        service.ModifiedDate = DateTime.Now;
                        service.Service_area = s.Service_area;
                        dbContext.SaveChanges();
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        if (services == 0)
                        {
                            Service service = dbContext.Services.Single(x => x.ServiceId == id);
                            service.ServiceName = s.ServiceName;
                            service.ModifiedDate = DateTime.Now;
                            service.Service_area = s.Service_area;
                            dbContext.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("ServiceName", "ServiceName already exists!");
                        }

                    }
                }
            }
            catch
            {
                return View(s);
            }
            return View(s);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Delete(int id)
        {
            var result = dbContext.Services.Single(x => x.ServiceId == id);
            return View(result);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Service s)
        {
            try
            {
                var result = dbContext.Services.Single(x => x.ServiceId == id);
                dbContext.Services.Remove(result);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(s);
            }
        }
    }
}