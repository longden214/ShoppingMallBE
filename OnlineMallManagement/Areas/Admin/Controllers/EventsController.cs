using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    public class EventsController : Controller
    {
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();

        [Authorize(Roles = "Admin,User,Customer")]
        public ActionResult Index()
        {
            var result = dbContext.Events.ToList();
            return View(result);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Create()
        {
            ViewBag.Shop_ShoId = new SelectList(dbContext.Shops.ToList(), "ShoId", "ShopName");
            return View();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Event e)
        {
            ViewBag.Shop_ShoId = new SelectList(dbContext.Shops.ToList(), "ShoId", "ShopName");
            if (ModelState.IsValid)
            {
                try
                {
                    if (e.StartDate < e.EndDate)
                    {
                        e.CreatedDate = DateTime.Now;
                        e.ModifiedDate = DateTime.Now;
                        var result = dbContext.Events.Add(e);

                        dbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("StartDate", "Start must be lesser than EndTime");
                        ModelState.AddModelError("EndDate", "EndDate must be greater than StartTime");
                    }

                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Edit(int id)
        {
            var result = dbContext.Events.Single(x => x.Event_id == id);
            ViewBag.Shop_Id = new SelectList(dbContext.Shops.ToList(), "ShoId", "ShopName", result.Shop_ShoId);

            return View(result);

        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Event e)
        {
            try
            {
                ViewBag.Shop_Id = new SelectList(dbContext.Shops.ToList(), "ShoId", "ShopName", e.Shop_ShoId);
                if (ModelState.IsValid)
                {
                    if (e.StartDate < e.EndDate)
                    {
                        Event events = dbContext.Events.Single(x => x.Event_id == id);
                        events.Shop_ShoId = e.Shop_ShoId;
                        events.StartDate = e.StartDate;
                        events.Price = e.Price;
                        events.Descriptions = e.Descriptions;
                        events.EndDate = e.EndDate;
                        events.Image = e.Image;
                        events.Address = e.Address;
                        events.Title = e.Title;
                        events.slug = e.slug;
                        events.Meta_title = e.Meta_title;
                        events.Meta_keyword = e.Meta_keyword;
                        events.Meta_description = e.Meta_description;
                        events.ModifiedDate = DateTime.Now;
                        events.Status = e.Status;
                        dbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("StartDate", "Start must be lesser than EndTime");
                        ModelState.AddModelError("EndDate", "EndDate must be greater than StartTime");
                    }
                }
               
            }
            catch
            {
                return View(e);
            }

            return View(e);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Delete(int id)
        {
            var result = dbContext.Events.Single(x => x.Event_id == id);
            ViewBag.Shop_Id = new SelectList(dbContext.Shops.ToList(), "ShoId", "ShopName", result.Shop_ShoId);
            return View(result);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Shop s)
        {
            try
            {
                var result = dbContext.Events.Single(x => x.Event_id == id);
                dbContext.Events.Remove(result);
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