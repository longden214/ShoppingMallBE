using OnlineMallManagement.Areas.Admin.Data;
using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    public class OrderDetailsController : Controller
    {
        DBOnlineMallEntities dbcontext = new DBOnlineMallEntities();

        [Authorize(Roles = "Admin,User,Customer")]
        public ActionResult Index()
        {
            ViewBag.ShowId = dbcontext.Rooms.ToList();

            return View();
        }

        public JsonResult Load(string txtSearch, int? showId, string StartTime, string StartDate, string EndDate, int? page = 1)
        {
            using (DBOnlineMallEntities dbcontext = new DBOnlineMallEntities())
            {
                dbcontext.Configuration.ProxyCreationEnabled = false;

                if (txtSearch == null)
                {
                    txtSearch = "";
                }
                if (StartDate == "")
                {
                    StartDate = null;
                }
                if (EndDate == "")
                {
                    EndDate = null;
                }

                object[] sqlParams =
                {
                    new SqlParameter("@showId",showId ?? -1),
                    new SqlParameter("@search",txtSearch)
                };
                var detail = dbcontext.Database.SqlQuery<OrderDetailsModel>("EXEC OrderDetailFilter @showId, @search", sqlParams).ToList();

                if (StartTime != null && StartTime != "")
                {
                    detail = detail.Where(x => x.StartTime.Equals(StartTime)).ToList();
                }


                if (StartDate == null && EndDate != null)
                {
                    detail = detail.Where(x => DateTime.Parse(x.ScreeningDate) <= DateTime.Parse(EndDate)).ToList();
                }
                else if (StartDate != null && EndDate == null)
                {
                    detail = detail.Where(x => DateTime.Parse(x.ScreeningDate) >= DateTime.Parse(StartDate)).ToList();
                }
                else if (StartDate != null && EndDate != null)
                {
                    detail = detail.Where(x => DateTime.Parse(x.ScreeningDate) <= DateTime.Parse(EndDate) && DateTime.Parse(x.ScreeningDate) >= DateTime.Parse(StartDate)).ToList();
                }

                page = page ?? 1;

                int pageSize = 15;
                int start = (int)(page - 1) * pageSize;

                ViewBag.pageCurrent = page;
                int totalPage = detail.Count();
                float totalNumsize = (totalPage / (float)pageSize);
                int numSize = (int)Math.Ceiling(totalNumsize);
                ViewBag.numSize = numSize;
                var dataPost = detail.Skip(start).Take(pageSize);

                List<OrderDetailsModel> listPost = new List<OrderDetailsModel>();
                listPost = dataPost.ToList();

                return Json(new { detail = listPost, pageCurrent = page, numSize = numSize }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}