﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        [Authorize(Roles = "Admin,User,Customer")]
        public ActionResult Index()
        {
            return View();
        }
    }
}