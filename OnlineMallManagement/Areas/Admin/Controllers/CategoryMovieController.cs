using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    [Authorize]
    public class CategoryMovieController : Controller
    {
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();
        // GET: Admin/CategoryMovie
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult loadData(string search, int? page, int? pageSize)
        {
            if (search == null)
            {
                search = "";
            }

            var CategoryList = dbContext.Category_Movie.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();

            var _pageSize = pageSize ?? 8;
            var pageIndex = page ?? 1;
            var totalPage = CategoryList.Count();
            var numberPage = Math.Ceiling((double)totalPage / _pageSize);

            var data = CategoryList.Skip((pageIndex - 1) * _pageSize).Take(_pageSize);

            return Json(new
            {
                proList = data,
                TotalItems = totalPage,
                CurrentPage = pageIndex,
                NumberPage = numberPage,
                PageSize = _pageSize
            }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetById(int id)
        {
            dbContext.Configuration.ProxyCreationEnabled = false;
            var item = dbContext.Category_Movie.Where(c => c.Cate_id == id).FirstOrDefault();
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Category/Create
        [HttpPost]
        public ActionResult Create(Category_Movie model)
        {
            if (model.Cate_id > 0)
            {
                var item = dbContext.Category_Movie.Find(model.Cate_id);
                item.Name = model.Name;
                item.slug = model.slug;
                item.ModifiedDate = DateTime.Now;
                item.Meta_title = model.Meta_title;
                item.Meta_keyword = model.Meta_keyword;
                item.Meta_description = model.Meta_description;

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
                dbContext.Category_Movie.Add(model);
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

        // POST: Admin/Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var cate = dbContext.Category_Movie.Where(x => x.Cate_id == id).FirstOrDefault();
                dbContext.Category_Movie.Remove(cate);

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