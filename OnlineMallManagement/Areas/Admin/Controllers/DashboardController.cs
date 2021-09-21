using OnlineMallManagement.Areas.Admin.Data;
using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();

        [Authorize(Roles = "Admin,User,Customer")]
        public ActionResult Index()
        {
            HttpContext.Application["favicon"] = dbContext.Configs.Find(15).value;
            HttpContext.Application["logo"] = dbContext.Configs.Find(16).value;

            ViewBag.User = dbContext.Admins.Count();
            ViewBag.Shop = dbContext.Shops.Where(x => x.Service.Service_area == 1 || x.Service.Service_area == 9).Count();
            ViewBag.Movie = dbContext.Movies.Count();

            ViewBag.Revenue = dbContext.Orders.Where(x => x.OrderDate.Value.Year == DateTime.Now.Year && x.OrderDate.Value.Month == DateTime.Now.Month).Sum(x => x.Total);

            var topMovie = (from sn in dbContext.Screenings
                            join ss in dbContext.Screening_seat on sn.Id equals ss.Screening_Id
                            join mv in dbContext.Movies on sn.Movie_Id equals mv.IdMovie
                            group sn by sn.Movie_Id into c
                            select new TopMovieView
                            {
                                MovieId = c.FirstOrDefault().Movie_Id,
                                MovieImage = c.FirstOrDefault().Movie.Image,
                                MovieName = c.FirstOrDefault().Movie.MoviveName,
                                TotalTicket = c.Count()
                            }).OrderByDescending(x => x.TotalTicket).ToList().Take(10);
            ViewBag.TopMovie = topMovie;

            return View();
        }
    }
}