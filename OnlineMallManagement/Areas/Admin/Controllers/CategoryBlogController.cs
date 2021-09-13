using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    public class CategoryBlogController : Controller
    {
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();

        [Authorize(Roles = "Admin,User,Customer")]
        public ActionResult Index()
        {
            var result = dbContext.Category_blog.ToList();
            return View(result);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category_blog c)
        {
            var categoryblog = dbContext.Category_blog.Where(x => x.name.Equals(c.name)).Count();
            try
            {
                if (ModelState.IsValid)
                {
                    if (categoryblog == 0)
                    {
                        c.CreatedDate = DateTime.Now;
                        c.ModifiedDate = DateTime.Now;
                        var result = dbContext.Category_blog.Add(c);
                        dbContext.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("name", "Name already exists!");
                    }
                }

            }
            catch
            {
                return View();
            }
            return View();
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Edit(int id)
        {
            var cate = dbContext.Category_blog.Single(x => x.Cate_blog_id == id);
            return View(cate);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category_blog c)
        {
            var categoryblog = dbContext.Category_blog.Where(x => x.name.Equals(c.name)).Count();
            var category = dbContext.Category_blog.Find(id);
            try
            {
                if (ModelState.IsValid)
                {
                    if (category.name == c.name)
                    {
                        Category_blog cate = dbContext.Category_blog.Single(x => x.Cate_blog_id == id);
                        cate.Meta_title = c.Meta_title;
                        cate.Meta_keyword = c.Meta_keyword;
                        cate.Meta_description = c.Meta_description;
                        cate.ModifiedDate = DateTime.Now;
                        dbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (categoryblog == 0)
                        {
                            Category_blog cate = dbContext.Category_blog.Single(x => x.Cate_blog_id == id);
                            cate.name = c.name;
                            cate.Meta_title = c.Meta_title;
                            cate.Meta_keyword = c.Meta_keyword;
                            cate.Meta_description = c.Meta_description;
                            cate.slug = c.slug;
                            cate.ModifiedDate = DateTime.Now;
                            dbContext.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("name", "Name already exists!");
                        }
                    }
                }

            }
            catch
            {
                return View(c);
            }
            return View(c);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Delete(int id)
        {
            var c = dbContext.Category_blog.Single(x => x.Cate_blog_id == id);
            return View(c);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Category_blog c)
        {
            try
            {
                var result = dbContext.Category_blog.Single(x => x.Cate_blog_id == id);
                dbContext.Category_blog.Remove(result);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = string.Format("You cant remove!!!");
                return View(c);

            }
        }

    }
}