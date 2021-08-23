using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Controllers
{
    public class EventController : Controller
    {
        public ActionResult Events()
        {
            return View();
        }
        // GET: Event
        public ActionResult EventSingle()
        {
            return View();
        }
    }
}