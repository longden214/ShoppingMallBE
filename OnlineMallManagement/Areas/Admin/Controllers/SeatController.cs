using OnlineMallManagement.Areas.Admin.Data;
using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    
    public class SeatController : Controller
    {
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();

        [Authorize(Roles = "Admin,User,Customer")]
        public ActionResult Index()
        {
            ViewBag.Rooms = dbContext.Rooms.ToList();
            return View();
        }

        [Authorize(Roles = "Admin,User,Customer")]
        [HttpGet]
        public JsonResult loadData(int hallId, int? page, int? pageSize)
        {

            var seatList = (from s in dbContext.Seats
                                join h in dbContext.Rooms on s.IdRoom equals h.IdRoom
                                select new SeatViewModel
                                {
                                    SeatId = s.IdSeat,
                                    SeatName = s.SeatName,
                                    SeatPrice = (double)s.price,
                                    SeatHall = h.RoomName
                                }).ToList();
            if (hallId > 0)
            {
                seatList = (from s in dbContext.Seats
                            join h in dbContext.Rooms on s.IdRoom equals h.IdRoom
                            where s.IdRoom == hallId
                            select new SeatViewModel
                            {
                                SeatId = s.IdSeat,
                                SeatName = s.SeatName,
                                SeatPrice = (double)s.price,
                                SeatHall = h.RoomName
                            }).ToList();
            }

            var _pageSize = pageSize ?? 8;
            var pageIndex = page ?? 1;
            var totalPage = seatList.Count();
            var numberPage = Math.Ceiling((double)totalPage / _pageSize);

            var data = seatList.Skip((pageIndex - 1) * _pageSize).Take(_pageSize);

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
        [HttpPost]
        public ActionResult Create(Seat model)
        {
            if (model.IdSeat > 0)
            {
                var single = dbContext.Seats.Where(x => x.SeatName.Equals(model.SeatName) && x.IdRoom == model.IdRoom).Count();
                if (single > 0)
                {
                    return Json(new { success = false, edit = true });
                }

                var item = dbContext.Seats.Find(model.IdSeat);
                item.SeatName = model.SeatName;
                item.IdRoom = model.IdRoom;
                item.price = model.price;
                item.ModifiedDate = DateTime.Now;

                dbContext.Entry(item).State = EntityState.Modified;
                try
                {
                    dbContext.SaveChanges();

                    var editRoom = dbContext.Rooms.Where(x => x.IdRoom == model.IdRoom).FirstOrDefault();
                    editRoom.TotalSeat = dbContext.Seats.Where(x => x.IdRoom == model.IdRoom).Count();
                    editRoom.ModifiedDate = DateTime.Now;

                    dbContext.Entry(editRoom).State = EntityState.Modified;
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
                var single = dbContext.Seats.Where(x => x.SeatName.Equals(model.SeatName) && x.IdRoom == model.IdRoom).Count();
                if (single > 0)
                {
                    return Json(new { success = false, edit = false });
                }

                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                dbContext.Seats.Add(model);
                try
                {
                    dbContext.SaveChanges();

                    var editRoom = dbContext.Rooms.Where(x => x.IdRoom == model.IdRoom).FirstOrDefault();
                    editRoom.TotalSeat = dbContext.Seats.Where(x => x.IdRoom == model.IdRoom).Count();
                    editRoom.ModifiedDate = DateTime.Now;

                    dbContext.Entry(editRoom).State = EntityState.Modified;
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
            var item = dbContext.Seats.Where(c => c.IdSeat == id).FirstOrDefault();
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var cate = dbContext.Seats.Where(x => x.IdSeat == id).FirstOrDefault();
                dbContext.Seats.Remove(cate);

                dbContext.SaveChanges();

                var editRoom = dbContext.Rooms.Where(x => x.IdRoom == cate.IdRoom).FirstOrDefault();
                editRoom.TotalSeat = dbContext.Seats.Where(x => x.IdRoom == cate.IdRoom).Count();
                editRoom.ModifiedDate = DateTime.Now;

                dbContext.Entry(editRoom).State = EntityState.Modified;
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