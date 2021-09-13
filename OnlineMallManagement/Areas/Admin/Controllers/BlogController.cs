using OnlineMallManagement.Areas.Admin.Data;
using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    public class BlogController : Controller
    {

        [Authorize(Roles = "Admin,User,Customer")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin,User,Customer")]
        public JsonResult Load(string txtSearch, int? page = 1)
        {
            using (DBOnlineMallEntities dbcontext = new DBOnlineMallEntities())
            {
                dbcontext.Configuration.ProxyCreationEnabled = false;

                if (txtSearch == null)
                {
                    txtSearch = "";
                }

                var blog = (from d in dbcontext.Blogs
                            join ct in dbcontext.Category_blog on d.Category_blog_id equals ct.Cate_blog_id
                            where d.Title.Contains(txtSearch)
                            select new BlogViewModel
                            {
                                BlogId = d.BlogId,
                                BlogTime = d.BlogTime.ToString(),
                                Title = d.Title,
                                Images = d.Images,
                                Status = (bool)d.Status,
                                CategoryBlog = d.Category_blog.name
                            }).ToList();


                page = page ?? 1;

                int pageSize = 8;
                int start = (int)(page - 1) * pageSize;

                ViewBag.pageCurrent = page;
                int totalPage = blog.Count();
                float totalNumsize = (totalPage / (float)pageSize);
                int numSize = (int)Math.Ceiling(totalNumsize);
                ViewBag.numSize = numSize;
                var dataPost = blog.OrderByDescending(x => x.BlogId).Skip(start).Take(pageSize);

                List<BlogViewModel> listPost = new List<BlogViewModel>();
                listPost = dataPost.ToList();

                var check = true;
                string[] roleNames = Roles.GetRolesForUser();
                if (roleNames[0].Contains("Customer") && roleNames.Length == 1)
                {
                    check = false;
                }

                return Json(new { blog = listPost, pageCurrent = page, numSize = numSize,checkRole = check }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadCategory()
        {

            using (DBOnlineMallEntities dbcontext = new DBOnlineMallEntities())
            {
                return Json(dbcontext.Category_blog.Select(x => new
                {
                    CategoryId = x.Cate_blog_id,
                    CategoryName = x.name
                }).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetById(int? id)
        {
            using (DBOnlineMallEntities dbcontext = new DBOnlineMallEntities())
            {
                dbcontext.Configuration.ProxyCreationEnabled = false;

                var result = (from d in dbcontext.Blogs
                              where d.BlogId == id
                              select new BlogViewModel
                              {
                                  BlogId = d.BlogId,
                                  Description = d.Description,
                                  BlogTime = d.BlogTime.ToString(),
                                  Title = d.Title,
                                  Status = (bool)d.Status,
                                  Category_blog_id = d.Category_blog_id,
                                  Meta_title = d.Meta_title,
                                  Meta_keyword = d.Meta_keyword,
                                  Meta_description = d.Meta_description,
                                  Content = d.Content,
                                  slug = d.slug,
                                  Images = d.Images
                              }).FirstOrDefault();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Admin,User")]
        public JsonResult StoreOrEdit(Blog blog)
        {
            using (DBOnlineMallEntities dbcontext = new DBOnlineMallEntities())
            {
                if (blog.BlogId > 0)
                {
                    var item = dbcontext.Blogs.Find(blog.BlogId);
                    item.Description = blog.Description;
                    item.BlogTime = blog.BlogTime;
                    item.Category_blog_id = blog.Category_blog_id;
                    item.Title = blog.Title;
                    item.Content = blog.Content;
                    item.Status = blog.Status;
                    item.slug = blog.slug;
                    item.Meta_title = blog.Meta_title;
                    item.Meta_keyword = blog.Meta_keyword;
                    item.Meta_description = blog.Meta_description;
                    item.ModifiedDate = DateTime.Now;
                    item.Images = blog.Images;

                    dbcontext.Entry(item).State = EntityState.Modified;
                    dbcontext.SaveChanges();
                    return Json(new { success = true, edit = true });
                }
                else
                {
                    blog.CreatedDate = DateTime.Now;
                    blog.ModifiedDate = DateTime.Now;
                    dbcontext.Blogs.Add(blog);
                    dbcontext.SaveChanges();
                    return Json(new { success = true, edit = true });
                }


            }
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (DBOnlineMallEntities dbcontext = new DBOnlineMallEntities())
            {
                var b = dbcontext.Blogs.Where(x => x.BlogId == id).FirstOrDefault<Blog>();
                dbcontext.Blogs.Remove(b);
                dbcontext.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully", JsonRequestBehavior.AllowGet });
            }
        }

    }
}