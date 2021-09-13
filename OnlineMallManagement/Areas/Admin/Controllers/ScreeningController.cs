using OnlineMallManagement.Areas.Admin.Data;
using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    
    public class ScreeningController : Controller
    {
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();

        [Authorize(Roles = "Admin,User,Customer")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin,User,Customer")]
        public JsonResult loadData(string search, int? page, int? pageSize)
        {
            if (search == null)
            {
                search = "";
            }

            var ProductList = (from s in dbContext.Screenings
                               join mv in dbContext.Movies on s.Movie_Id equals mv.IdMovie
                               join r in dbContext.Rooms on s.Room_IdRoom equals r.IdRoom
                               where mv.MoviveName.Contains(search)
                               orderby s.Id descending
                               select new ScreeningViewModel
                               {
                                   ScreeningId = s.Id,
                                   MovieImg = mv.Image,
                                   MovieName = mv.MoviveName,
                                   CinemaHall = r.RoomName,
                                   Date = s.ScreeningDate.ToString(),
                                   Time = s.StartTime.ToString().Remove(5),
                                   Status = (bool)s.Status
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

        [Authorize(Roles = "Admin,User")]
        public ActionResult Create()
        {
            ViewBag.Movies = dbContext.Movies.Where(x => x.Status == true).ToList();
            ViewBag.Rooms = dbContext.Rooms.ToList();
            return View();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public JsonResult SaveScreening(Screening model)
        {
            try
            {
                if (model != null)
                {
                    model.CreatedDate = DateTime.Now;
                    model.ModifiedDate = DateTime.Now;

                    dbContext.Screenings.Add(model);
                    dbContext.SaveChanges();
                }

                return Json(new { result = true, newurl = Url.Action("Index", "Screening") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Edit(int ScreeningId)
        {
            ViewBag.Movies = dbContext.Movies.Where(x => x.Status == true).ToList();
            ViewBag.Rooms = dbContext.Rooms.ToList();
            ViewBag.Screening = dbContext.Screenings.Find(ScreeningId);

            return View();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public JsonResult EditScreening(Screening model)
        {
            try
            {
                if (model != null)
                {

                    var sr = dbContext.Screenings.Find(model.Id);
                    sr.Room_IdRoom = model.Room_IdRoom;
                    sr.Movie_Id = model.Movie_Id;
                    sr.ScreeningDate = model.ScreeningDate;
                    sr.StartTime = model.StartTime;
                    sr.ModifiedDate = DateTime.Now;

                    dbContext.Entry(sr).State = EntityState.Modified;
                    dbContext.SaveChanges();


                    return Json(new { Result = true, newurl = Url.Action("Index", "Screening") }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public JsonResult DeleteScreening(int id)
        {
            var result = false;
            try
            {

                var Item = dbContext.Screenings.Find(id);
                dbContext.Screenings.Remove(Item);
                dbContext.SaveChanges();

                result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CheckTime(int _roomId, DateTime _date, TimeSpan _time)
        {

            var data = dbContext.Screenings.Where(x => x.Room_IdRoom == _roomId && x.ScreeningDate == _date && x.StartTime == _time).Count();
            if (data > 0)
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}