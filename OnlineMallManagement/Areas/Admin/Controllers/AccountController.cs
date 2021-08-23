using OnlineMallManagement.Areas.Admin.Data;
using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
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

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
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
                    Session["username"] = data.FirstOrDefault().UserName;
                    Session["id"] = data.FirstOrDefault().Id;

                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản không chính xác!");
                }
            }

            return View(model);
        }
    }
}