using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    public class ConfigController : Controller
    {
        private DBOnlineMallEntities db;

        public ConfigController()
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
            return Json(db.Configs.AsEnumerable(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult getbyID(int Id)
        {
            var Config = db.Configs.FirstOrDefault(x => x.Id == Id);
            return Json(Config, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin,User")]
        public JsonResult Update(Config b)
        {
            try
            {
                Config config = db.Configs.FirstOrDefault(x => x.Id == b.Id);
                if (config != null)
                {
                    config.value = b.value;
                    config.Status = b.Status;

                    config.ModifiedDate = DateTime.Now;
                    db.SaveChanges();

                    HttpContext.Application["favicon"] = db.Configs.Find(15).value;
                    HttpContext.Application["logo"] = db.Configs.Find(16).value;

                    return Json(new { result = true,logo = HttpContext.Application["logo"].ToString() }, JsonRequestBehavior.AllowGet);
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