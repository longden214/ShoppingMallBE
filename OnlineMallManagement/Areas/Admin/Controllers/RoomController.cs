using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    
    public class RoomController : Controller
    {
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();

        [Authorize(Roles = "Admin,User,Customer")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin,User,Customer")]
        [HttpGet]
        public JsonResult loadData()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;
            var _roomList = dbContext.Rooms.ToList();
            
            return Json(new
            {
                RoomList = _roomList,
                TotalItems = _roomList.Count()
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public ActionResult Create(Room model)
        {
            if (model.IdRoom > 0)
            {
                var item = dbContext.Rooms.Find(model.IdRoom);
                item.RoomName = model.RoomName;
                item.ModifiedDate = DateTime.Now;

                dbContext.Entry(item).State = EntityState.Modified;
                try
                {
                    dbContext.SaveChanges();

                    return Json(new { success = true, edit = true });
                }
                catch (Exception)
                {
                    return Json(new { success = false, edit = true });
                }
            }
            else
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                dbContext.Rooms.Add(model);
                try
                {
                    dbContext.SaveChanges();
                    return Json(new { success = true, edit = false });
                }
                catch (Exception)
                {
                    return Json(new { success = false, edit = false });
                }
            }
        }

        public JsonResult GetById(int id)
        {
            dbContext.Configuration.ProxyCreationEnabled = false;
            var item = dbContext.Rooms.Where(c => c.IdRoom == id).FirstOrDefault();
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var cate = dbContext.Rooms.Where(x => x.IdRoom == id).FirstOrDefault();
                dbContext.Rooms.Remove(cate);

                dbContext.SaveChanges();
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
    }
}