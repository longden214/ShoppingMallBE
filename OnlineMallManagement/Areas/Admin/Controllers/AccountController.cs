using OnlineMallManagement.Areas.Admin.Data;
using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {

        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();
        private AccountModel accModel = new AccountModel();

        [Authorize(Roles = "Admin,User,Customer")]
        public ActionResult Index()
        {
            var name = HttpContext.Application["uName"].ToString();
            ViewBag.Admin = dbContext.Admins.FirstOrDefault(x => x.UserName == name);

            return View();
        }

        public ActionResult Login()
        {
            var favicon = dbContext.Configs.Find(15).value;
            ViewBag.Favicon = favicon;

            var logo = dbContext.Configs.Find(16).value;
            ViewBag.Logo = logo;

            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            HttpContext.Application.Clear();
            return RedirectToAction("Login", "Account");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var passMd5h = accModel.MD5Hash(model.password);
                var data = dbContext.Admins.Where(x => x.UserName == model.username && x.Password == passMd5h).ToList();
                if (data.Count() > 0)
                {
                    FormsAuthentication.SetAuthCookie(model.username, model.RememberMe);
                    
                    HttpContext.Application["uName"] = data.FirstOrDefault().UserName;
                    HttpContext.Application["uId"] = data.FirstOrDefault().Id;
                    HttpContext.Application["displayName"] = data.FirstOrDefault().DisplayName;

                    HttpContext.Application["favicon"] = dbContext.Configs.Find(15).value;
                    HttpContext.Application["logo"] = dbContext.Configs.Find(16).value;

                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect account!");
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Admin,User,Customer")]
        [HttpPost]
        public ActionResult EditAccount(Models.Admin model)
        {
            
            var uid = Convert.ToInt32(HttpContext.Application["uId"].ToString());
            var item = dbContext.Admins.Find(uid);
            item.UserName = model.UserName;
            item.DisplayName = model.DisplayName;
            item.Email = model.Email;
            item.Phone = model.Phone;
            dbContext.Entry(item).State = EntityState.Modified;
            try
            {
                HttpContext.Application["displayName"] = model.DisplayName;
                dbContext.SaveChanges();

                return Json(new { success = true, displayName = model.DisplayName });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        [Authorize(Roles = "Admin,User,Customer")]
        [HttpPost]
        public ActionResult ChangePassword(Models.Admin model)
        {
            var uid = Convert.ToInt32(HttpContext.Application["uId"].ToString());
            var item = dbContext.Admins.Find(uid);
            item.Password = new AccountModel().MD5Hash(model.Password);
            dbContext.Entry(item).State = EntityState.Modified;
            try
            {
                dbContext.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        [Authorize(Roles = "Admin,User,Customer")]
        [HttpPost]
        public JsonResult ValidationPassword(string pass)
        {
            var uid = Convert.ToInt32(HttpContext.Application["uId"].ToString());
            var _pass = new AccountModel().MD5Hash(pass);

            var data = dbContext.Admins.Where(x => x.Id == uid && x.Password == _pass).ToList();
            if (data.Count() > 0)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}