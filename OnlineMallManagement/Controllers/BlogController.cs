using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Controllers
{
    public class BlogController : Controller
    {
        public ActionResult BlogPost()
        {
            return View();
        }

        public ActionResult BlogClassic()
        {
            return View();
        }
    }
}