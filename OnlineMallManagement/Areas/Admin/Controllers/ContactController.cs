using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    public class ContactController : Controller
    {
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();

        [Authorize(Roles = "Admin,User,Customer")]
        public ActionResult Index()
        {
            ViewBag.Feedbacks = dbContext.Feedbacks.ToList();
            return View();
        }

        public JsonResult CreateFeedback(Feedback model)
        {
            try
            {
                model.FBDate = DateTime.Now;
                model.Status = true;
                dbContext.Feedbacks.Add(model);
                dbContext.SaveChanges();
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Admin,User")]
        public JsonResult DeleteFeedback(string[] arr)
        {
            try
            {
                foreach (var item in arr)
                {
                    int cv = Convert.ToInt32(item);
                    var find = dbContext.Feedbacks.Find(cv);
                    dbContext.Feedbacks.Remove(find);
                }

                dbContext.SaveChanges();
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult FindFeedback(int id)
        {
            var fb = dbContext.Feedbacks.Find(id);
            fb.Status = false;
            dbContext.Entry(fb).State = EntityState.Modified;
            dbContext.SaveChanges();

            var feedback = (from f in dbContext.Feedbacks
                           where f.Id == id
                           select new FeedbackViewModel
                           {
                               Id = f.Id,
                               FirstName = f.FirstName,
                               LastName = f.LastName,
                               Email = f.Email,
                               Phone = f.Phone,
                               Content = f.Content,
                               FBDate = f.FBDate.ToString(),
                               Status = f.Status
                           }).FirstOrDefault();

            return Json(new { model = feedback }, JsonRequestBehavior.AllowGet);

        }
    }
}