using OnlineMallManagement.Areas.Admin.Data;
using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Controllers
{
    public class MovieController : Controller
    {
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();
        // GET: Movie
        public ActionResult MovieCheckout(int? showId)
        {
            if (showId == null)
            {
                return RedirectToAction("MovieList");
            }

            ViewBag.Banner = dbContext.Banners.Find(23);

            var screening = dbContext.Screenings.Find(showId);

            ViewBag.MovieId = screening.Movie_Id;

            var movie = dbContext.Movies.Find(screening.Movie_Id);

            ViewBag.MovieName = movie.MoviveName;
            ViewBag.MovieLanguage = movie.Language;
            ViewBag.Day = screening.ScreeningDate.ToString("dd-MM-yyyy");
            ViewBag.Time = screening.StartTime.ToString("hh':'mm");

            var cart = Session["CartSession"];

            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                var seat = "";
                double total = 0;

                foreach (var item in list)
                {
                    seat += item.SeatName + ", ";
                    total += item.SeatPrice;
                }

                var seatSlice = seat.Remove(seat.Length - 2);

                ViewBag.Seats = seatSlice;
                ViewBag.SubTotal = total;
                ViewBag.Total = total + 3;
            }
            else
            {
                ViewBag.Seats = "";
                ViewBag.SubTotal = 0;
                ViewBag.Total = 0;
            }

            return View();
        }

        public JsonResult loadData(string search, int? page, int? pageSize, DateTime? date)
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            if (search == null)
            {
                search = "";
            }

            var ProductList = (from p in dbContext.Movies
                               where p.MoviveName.Contains(search) && p.Status == true
                               orderby p.IdMovie descending
                               select new MovieViewModel
                               {
                                   _IdMovie = p.IdMovie,
                                   _MoviveName = p.MoviveName,
                                   _Image = p.Image,
                                   _Duration = p.Duration,
                                   _AgeRestriction = p.AgeRestriction,
                                   _ReleaseDate = p.ReleaseDate.ToString(),
                                   _Actors = p.Actors,
                                   _Banner = p.banner,
                                   _Photos = p.photos,
                                   _Language = p.Language,
                                   _Description = p.Description,
                                   _Country = p.Country,
                                   _Status = (bool)p.Status
                               }).ToList();

            if (date != null)
            {
                var convert = date.Value.ToString("yyyy-MM-dd");

                ProductList = (from p in dbContext.Movies
                               join s in dbContext.Screenings on p.IdMovie equals s.Movie_Id
                               where p.MoviveName.Contains(search) && p.Status == true
                               where s.ScreeningDate.ToString().Equals(convert)
                               orderby p.IdMovie descending
                               group s by s.Movie_Id into c
                               select new MovieViewModel
                               {
                                   _IdMovie = c.FirstOrDefault().Movie.IdMovie,
                                   _MoviveName = c.FirstOrDefault().Movie.MoviveName,
                                   _Image = c.FirstOrDefault().Movie.Image,
                                   _Duration = c.FirstOrDefault().Movie.Duration,
                                   _AgeRestriction = c.FirstOrDefault().Movie.AgeRestriction,
                                   _ReleaseDate = c.FirstOrDefault().Movie.ReleaseDate.ToString(),
                                   _Actors = c.FirstOrDefault().Movie.Actors,
                                   _Banner = c.FirstOrDefault().Movie.banner,
                                   _Photos = c.FirstOrDefault().Movie.photos,
                                   _Language = c.FirstOrDefault().Movie.Language,
                                   _Description = c.FirstOrDefault().Movie.Description,
                                   _Country = c.FirstOrDefault().Movie.Country,
                                   _Status = (bool)c.FirstOrDefault().Movie.Status,
                                   _Count = c.Count()
                               }).ToList();
            }

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

        public JsonResult GetGenres(int MovieId)
        {
            dbContext.Configuration.ProxyCreationEnabled = false;
            var list = (from mt in dbContext.Movie_type
                        join g in dbContext.Category_Movie on mt.cate_movie_id equals g.Cate_id
                        join m in dbContext.Movies on mt.movie_id equals m.IdMovie
                        where mt.movie_id == MovieId
                        select g).ToList();

            return Json(new { genreList = list }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MovieDetail(int? id)
        {

            if (id == null || id == 0)
            {
                return RedirectToAction("MovieList");
            }

            var Movie = (from p in dbContext.Movies
                         where p.IdMovie == id
                         select new MovieViewModel
                         {
                             _IdMovie = p.IdMovie,
                             _MoviveName = p.MoviveName,
                             _Image = p.Image,
                             _Duration = p.Duration,
                             _AgeRestriction = p.AgeRestriction,
                             _ReleaseDate = p.ReleaseDate.ToString(),
                             _Actors = p.Actors,
                             _Country = p.Country,
                             _Banner = p.banner,
                             _Language = p.Language,
                             _Description = p.Description,
                             _Photos = p.photos,
                             _Status = (bool)p.Status
                         }).FirstOrDefault();

            var genres = (from mt in dbContext.Movie_type
                          join g in dbContext.Category_Movie on mt.cate_movie_id equals g.Cate_id
                          join m in dbContext.Movies on mt.movie_id equals m.IdMovie
                          where mt.movie_id == id
                          select g).ToList();

            List<string> list = new List<string>();

            foreach (var item in genres)
            {
                list.Add(item.Name);
            }

            ViewBag.genreList = list;

            return View(Movie);
        }

        public ActionResult MovieList()
        {
            ViewBag.Banner = dbContext.Banners.Find(26);
            return View();
        }

        public ActionResult MovieSeat(int? showId,int? movieId)
        {
            if (showId == null || movieId == null)
            {
                return RedirectToAction("MovieList");
            }

            ViewBag.Banner = dbContext.Banners.Find(27);

            ViewBag.MovieId = movieId;

            var screening = dbContext.Screenings.Find(showId);
            var movie = dbContext.Movies.Find(movieId);

            ViewBag.MovieName = movie.MoviveName;
            ViewBag.Day = screening.ScreeningDate.ToString("dd-MM-yyyy");
            ViewBag.Time = screening.StartTime.ToString("hh':'mm");

            var seats = dbContext.Seats.Where(x => x.IdRoom == screening.Room_IdRoom).ToList();

            ViewBag.SeatList = seats;
            ViewBag.ScreeningId = screening.Id;

            var seatStatus = dbContext.Screening_seat.Where(x => x.Screening_Id == screening.Id && x.Status == true).ToList();
            ViewBag.SeatStatus = seatStatus;
            return View();
        }

        public ActionResult MovieTicket(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("MovieList");
            }
            Session["CartSession"] = null;
            Session["Customer"] = null;

            string convert = DateTime.Now.ToString("yyyy-MM-dd");

            var times = (from p in dbContext.Movies
                         join s in dbContext.Screenings on p.IdMovie equals s.Movie_Id
                         where s.ScreeningDate.ToString().Equals(convert) && s.Movie_Id == id
                         select new ScreeningViewClient
                         {
                             Id = s.Id,
                             RoomId = s.Room_IdRoom,
                             MovieId = s.Movie_Id,
                             Day = s.ScreeningDate.ToString(),
                             Time = s.StartTime.ToString()
                         }).ToList();

            ViewBag.Times = times;
            ViewBag.Count = times.Count();
            ViewBag.MovieId = id;

            var bn = dbContext.Movies.Find(id).banner;

            if (bn.Length > 0)
            {
                ViewBag.MovieBanner = bn;
            }
            else
            {
                ViewBag.MovieBanner = "/Content/client/assets/images/banner/moviebaner2.jpg";
            }
            

            return View();
        }

        public JsonResult GetTimes(int id,DateTime? day)
        {
            string convert = day.Value.ToString("yyyy-MM-dd");

            var times = (from p in dbContext.Movies
                         join s in dbContext.Screenings on p.IdMovie equals s.Movie_Id
                         where s.ScreeningDate.ToString().Equals(convert) && s.Movie_Id == id
                         select new ScreeningViewClient
                         {
                             Id = s.Id,
                             RoomId = s.Room_IdRoom,
                             MovieId = s.Movie_Id,
                             Day = s.ScreeningDate.ToString(),
                             Time = s.StartTime.ToString()
                         }).ToList();

            return Json(new { timeList = times }, JsonRequestBehavior.AllowGet);
        }
    }
}