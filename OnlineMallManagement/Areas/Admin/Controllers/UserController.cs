using OnlineMallManagement.Areas.Admin.Data;
using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Areas.Admin.Controllers
{

    public class UserController : Controller
    {
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();
        
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.Roles = dbContext.Roles.ToList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        public JsonResult loadData(string search, int? page, int? pageSize)
        {

            int idLogin = (int)HttpContext.Application["uId"];

            var ProductList = (from ar in dbContext.AdminRoles
                               join a in dbContext.Admins on ar.Admin_id equals a.Id
                               join r in dbContext.Roles on ar.Role_id equals r.Role_id
                               where a.DisplayName.Contains(search)
                               where a.Id != idLogin
                               select new UserViewModel
                               {
                                   UserId = a.Id,
                                   DisplayName = a.DisplayName,
                                   Email = a.Email,
                                   Phone = a.Phone,
                                   RoleName = r.Role_name
                               }).ToList();

            var _pageSize = pageSize ?? 8;
            var pageIndex = page ?? 1;
            var totalPage = ProductList.Count();
            var numberPage = Math.Ceiling((double)totalPage / _pageSize);

            var data = ProductList.Skip((pageIndex - 1) * _pageSize).Take(_pageSize);
            return Json(new
            {
                proList = data,
                TotalItems = totalPage,
                CurrentPage = pageIndex,
                NumberPage = numberPage,
                PageSize = _pageSize
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult SaveUser(Models.Admin model, int _role)
        {
            try
            {
                if (model != null && _role != 0)
                {
                    model.Password = new AccountModel().MD5Hash(model.Password);
                    dbContext.Admins.Add(model);
                    dbContext.SaveChanges();

                    var newId = model.Id;

                    AdminRole ar = new AdminRole();
                    ar.Role_id = _role;
                    ar.Admin_id = newId;

                    dbContext.AdminRoles.Add(ar);
                    dbContext.SaveChanges();

                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckUserName(string uname)
        {
            var check = dbContext.Admins.Where(x => x.UserName.ToLower().Equals(uname.ToLower())).Count();

            if (check > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        public JsonResult Delete(int id)
        {
            try
            {
                var role = dbContext.AdminRoles.Where(x => x.Admin_id == id).FirstOrDefault();
                dbContext.AdminRoles.Remove(role);
                dbContext.SaveChanges();

                var item = dbContext.Admins.Find(id);
                dbContext.Admins.Remove(item);
                dbContext.SaveChanges();

                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}