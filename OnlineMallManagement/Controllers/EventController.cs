using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Controllers
{
    public class EventController : Controller
    {
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();
        public ActionResult Events()
        {
            ViewBag.Banner = dbContext.Banners.Find(14);
            var events = (from ev in dbContext.Events
                          where ev.Status == true
                          select ev).OrderByDescending(x => x.EndDate).ToList();

            ViewBag.Events = events;

            return View();
        }
        // GET: Event
        public ActionResult EventSingle(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Events");
            }

            ViewBag.Banner = dbContext.Banners.Find(17);
            ViewBag.Event = dbContext.Events.Find(id);

            ViewBag.EventOther = dbContext.Events.Where(x => x.Status == true && x.Event_id != id).Take(3);

            return View();
        }
    }
}