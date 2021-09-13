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
    
    public class MovieController : Controller
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

            var ProductList = (from p in dbContext.Movies
                               where p.MoviveName.Contains(search)
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
                                   _Country = p.Country,
                                   _Banner = p.banner,
                                   _Language = p.Language,
                                   _Description = p.Description,
                                   _Photos = p.photos,
                                   _Status = (bool)p.Status
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
            ViewBag.Genres = dbContext.Category_Movie.ToList();
            return View();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public JsonResult SaveMovie(Movie _movieData)
        {
            int movieid = 0;
            try
            {
                if (_movieData != null)
                {
                    _movieData.CreatedDate = DateTime.Now;
                    _movieData.ModifiedDate = DateTime.Now;

                    dbContext.Movies.Add(_movieData);
                    dbContext.SaveChanges();

                    movieid = _movieData.IdMovie;
                }

                return Json(new { movieid = movieid }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { movieid = movieid }, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public JsonResult SaveGenre(string[] genres, int movieId)
        {
            try
            {
                if (movieId > 0)
                {
                    foreach (string item in genres)
                    {
                        int convert = Int32.Parse(item);

                        Movie_type type = new Movie_type();
                        type.cate_movie_id = convert;
                        type.movie_id = movieId;

                        dbContext.Movie_type.Add(type);
                        dbContext.SaveChanges();
                    }
                    return Json(new { Result = true, newurl = Url.Action("Index", "Movie") }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Edit(int IdMovie)
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            ViewBag.Genres = dbContext.Category_Movie.ToList();
            var movie = dbContext.Movies.Find(IdMovie);

            ViewBag.MovieTypes = dbContext.Movie_type.Where(x => x.movie_id == IdMovie).ToList();

            return View(movie);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public JsonResult EditMovie(Movie _movieData)
        {
            int movieid = 0;
            try
            {
                if (_movieData != null)
                {
                    DeleteType(_movieData.IdMovie);

                    var mv = dbContext.Movies.Find(_movieData.IdMovie);
                    mv.MoviveName = _movieData.MoviveName;
                    mv.Image = _movieData.Image;
                    mv.Duration = _movieData.Duration;
                    mv.Actors = _movieData.Actors;
                    mv.AgeRestriction = _movieData.AgeRestriction;
                    mv.Description = _movieData.Description;
                    mv.Language = _movieData.Language;
                    mv.ReleaseDate = _movieData.ReleaseDate;
                    mv.Country = _movieData.Country;
                    mv.slug = _movieData.slug;
                    mv.Meta_title = _movieData.Meta_title;
                    mv.Meta_keyword = _movieData.Meta_keyword;
                    mv.Meta_description = _movieData.Meta_description;
                    mv.ModifiedDate = DateTime.Now;
                    mv.Status = _movieData.Status;
                    mv.banner = _movieData.banner;
                    mv.photos = _movieData.photos;

                    dbContext.Entry(mv).State = EntityState.Modified;
                    dbContext.SaveChanges();

                    movieid = _movieData.IdMovie;

                    return Json(new { movieid = movieid }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { movieid = movieid }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { movieid = movieid }, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "Admin,User")]
        public JsonResult EditStatus(int id)
        {
            bool result = false;
            try
            {

                var pro1 = dbContext.Movies.Find(id);
                var pro2 = dbContext.Movies.Find(id);
                if (pro1.Status == true)
                {
                    pro2.Status = false;
                }
                else
                {
                    pro2.Status = true;
                }

                dbContext.Entry(pro2).State = EntityState.Modified;
                dbContext.SaveChanges();
                result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Admin,User")]
        public void DeleteType(int idMovie)
        {
            var type = dbContext.Movie_type.Where(x => x.movie_id == idMovie).ToList();
            foreach (var item in type)
            {
                dbContext.Movie_type.Remove(item);
            }
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public JsonResult DeleteMovie(int id)
        {
            var result = false;
            try
            {
                DeleteType(id);

                var proItem = dbContext.Movies.Find(id);
                dbContext.Movies.Remove(proItem);
                dbContext.SaveChanges();

                result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}