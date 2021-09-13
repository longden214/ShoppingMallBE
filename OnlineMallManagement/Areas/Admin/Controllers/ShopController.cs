using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();

        [Authorize(Roles = "Admin,User,Customer")]
        public ActionResult Index(string SearchString, int? page)
        {

            var ressult = from s in dbContext.Shops select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                ressult = dbContext.Shops.Where(s => s.ShopName.Contains(SearchString));
            }

            if (page > 0)
            {
                page = page;
            }
            else
            {
                page = 1;
            }
            int limit = 8;
            int start = (int)(page - 1) * limit;
            int totalShop = ressult.Count();

            ViewBag.totalShop = totalShop;
            ViewBag.pageCurrent = page;

            float numberPage = (float)totalShop / limit;

            ViewBag.numberPage = (int)Math.Ceiling(numberPage);

            var dataShop = ressult.OrderByDescending(s => s.ShoId).Skip(start).Take(limit);
            
            return View(dataShop.ToList());
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Create()
        {
            ViewBag.Serviec_Id = new SelectList(dbContext.Services.ToList(), "ServiceId", "ServiceName");
            return View();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Shop s)
        {
            var shops = dbContext.Shops.Where(x => x.ShopName.Equals(s.ShopName)).Count();
            try
            {
                ViewBag.Serviec_Id = new SelectList(dbContext.Services.ToList(), "ServiceId", "ServiceName");
                if (ModelState.IsValid)
                {
                    if (shops == 0)
                    {
                        s.CreatedDate = DateTime.Now;
                        s.ModifiedDate = DateTime.Now;
                        var result = dbContext.Shops.Add(s);
                        dbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("ShopName", "ShopName already exists!");
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
            var result = dbContext.Shops.Single(x => x.ShoId == id);
            ViewBag.Serviec_Id = new SelectList(dbContext.Services.ToList(), "ServiceId", "ServiceName", result.ServiceId);

            return View(result);

        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Shop s)
        {
            var shops = dbContext.Shops.Where(x => x.ShopName.Equals(s.ShopName)).Count();
            var shopItem = dbContext.Shops.Find(id);
            try
            {
                ViewBag.Serviec_Id = new SelectList(dbContext.Services.ToList(), "ServiceId", "ServiceName", s.ServiceId);
                if (ModelState.IsValid)
                {
                    if (shopItem.ShopName == s.ShopName)
                    {
                        Shop shop = dbContext.Shops.Single(x => x.ShoId == id);
                        shop.Photo = s.Photo;
                        shop.ServiceId = s.ServiceId;
                        shop.Description = s.Description;
                        shop.Address = s.Address;
                        shop.Phone = s.Phone;
                        shop.Status = s.Status;
                        shop.Email = s.Email;
                        shop.Url_web = s.Url_web;
                        shop.Logo = s.Logo;
                        shop.slug = s.slug;
                        shop.Meta_title = s.Meta_title;
                        shop.Meta_keyword = s.Meta_keyword;
                        shop.Meta_description = s.Meta_description;
                        shop.ModifiedDate = DateTime.Now;
                        dbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (shops == 0)
                        {
                            Shop shop = dbContext.Shops.Single(x => x.ShoId == id);
                            shop.ShopName = s.ShopName;
                            shop.Photo = s.Photo;
                            shop.ServiceId = s.ServiceId;
                            shop.Description = s.Description;
                            shop.Address = s.Address;
                            shop.Phone = s.Phone;
                            shop.Status = s.Status;
                            shop.Email = s.Email;
                            shop.Url_web = s.Url_web;
                            shop.Logo = s.Logo;
                            shop.slug = s.slug;
                            shop.Meta_title = s.Meta_title;
                            shop.Meta_keyword = s.Meta_keyword;
                            shop.Meta_description = s.Meta_description;
                            shop.ModifiedDate = DateTime.Now;
                            dbContext.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("ShopName", "ShopName already exists!");
                        }
                    }

                }

            }
            catch
            {
                return View(s);
            }
            return View(s);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Delete(int id)
        {
            var result = dbContext.Shops.Single(x => x.ShoId == id);
            ViewBag.Serviec_Id = new SelectList(dbContext.Services.ToList(), "ServiceId", "ServiceName", result.ServiceId);
            return View(result);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Shop s)
        {
            try
            {
                var result = dbContext.Shops.Single(x => x.ShoId == id);
                dbContext.Shops.Remove(result);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(s);
            }
        }
    }
}