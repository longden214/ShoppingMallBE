using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    public class Shop_ProductController : Controller
    {
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();
        // GET: Admin/Shop_Product

        [Authorize(Roles = "Admin,User,Customer")]
        public ActionResult Index(int? Shopid)
        {
            /*var ressult = dbContext.Shop_Product.ToList();
            return View(ressult);*/

            if (Shopid == null)
            {
                return RedirectToAction("Index", "Shop");
            }

            Shopid = Shopid ?? 0;
            ViewBag.ShopId = Shopid;

            var result = dbContext.Shop_Product.Where(x => x.Shop_ShoId == Shopid).ToList();
            return View(result);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Create(int shopid)
        {
            ViewBag.ShopId = shopid;
            ViewBag.Shop_ShoId = new SelectList(dbContext.Shops.ToList(), "ShoId", "ShopName");
            return View();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Shop_Product sp, int shopId)
        {
            ViewBag.ShopId = shopId;
            var shopproduct = dbContext.Shop_Product.Where(x => x.Pro_name.Equals(sp.Pro_name) && x.Shop_ShoId == shopId).Count();
            try
            {
                ViewBag.Shop_ShoId = new SelectList(dbContext.Shops.ToList(), "ShoId", "ShopName");
                if (ModelState.IsValid)
                {
                    if (shopproduct == 0)
                    {
                        sp.CreatedDate = DateTime.Now;
                        sp.ModifiedDate = DateTime.Now;
                        sp.Shop_ShoId = shopId;
                        dbContext.Shop_Product.Add(sp);

                        dbContext.SaveChanges();
                        return RedirectToAction("Index", new { Shopid = sp.Shop_ShoId });
                    }
                    else
                    {
                        ModelState.AddModelError("Pro_name", "Pro_name already exists!");
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

            var result = dbContext.Shop_Product.Single(x => x.Pro_Id == id);
            ViewBag.Shop_Id = result.Shop_ShoId;

            return View(result);

        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Shop_Product sp)
        {
            var shopproduct = dbContext.Shop_Product.Where(x => x.Pro_name.Equals(sp.Pro_name)).Count();
            var shop = dbContext.Shop_Product.Find(id);
            ViewBag.Shop_Id = sp.Shop_ShoId;
            try
            {
                if (ModelState.IsValid)
                {

                    if (shop.Pro_name == sp.Pro_name)
                    {
                        Shop_Product sproduct = dbContext.Shop_Product.Single(x => x.Pro_Id == id);
                        sproduct.Images = sp.Images;
                        sproduct.Shop_ShoId = sp.Shop_ShoId;
                        sproduct.Status = sp.Status;
                        sproduct.ModifiedDate = DateTime.Now;

                        dbContext.SaveChanges();
                        return RedirectToAction("Index", new { Shopid = sp.Shop_ShoId });
                    }
                    else
                    {
                        if (shopproduct == 0)
                        {
                            Shop_Product sproduct = dbContext.Shop_Product.Single(x => x.Pro_Id == id);
                            sproduct.Pro_name = sp.Pro_name;
                            sproduct.Images = sp.Images;
                            sproduct.Shop_ShoId = sp.Shop_ShoId;
                            sproduct.Status = sp.Status;
                            sproduct.ModifiedDate = DateTime.Now;

                            dbContext.SaveChanges();
                            return RedirectToAction("Index", new { Shopid = sp.Shop_ShoId });
                        }
                        else
                        {
                            ModelState.AddModelError("Pro_name", "Pro_name already exists!");
                        }
                    }
                }

            }
            catch
            {
                return View(sp);
            }
            return View(sp);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Delete(int id)
        {
            var result = dbContext.Shop_Product.Single(x => x.Pro_Id == id);

            ViewBag.Shop_Id = result.Shop_ShoId;
            return View(result);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Shop_Product sp)
        {
            ViewBag.Shop_Id = sp.Shop_ShoId;
            try
            {
                var result = dbContext.Shop_Product.Single(x => x.Pro_Id == sp.Pro_Id);
                var shopid = result.Shop_ShoId;
                dbContext.Shop_Product.Remove(result);
                dbContext.SaveChanges();
                return RedirectToAction("Index", new { Shopid = shopid });
            }
            catch
            {
                return View(sp);
            }
        }
    }
}