using OnlineMallManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Areas.Admin.Controllers
{
    public class SalesController : Controller
    {
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();

        [Authorize(Roles = "Admin,User,Customer")]
        public ActionResult Index()
        {
            var result = dbContext.Sales.ToList();
            return View(result);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Create()
        {
            ViewBag.Shop_ShoId = new SelectList(dbContext.Shops.Where(x => x.Service.Service_area != 3).ToList(), "ShoId", "ShopName");
            return View();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sale s)
        {
            try
            {
                ViewBag.Shop_ShoId = new SelectList(dbContext.Shops.Where(x => x.Service.Service_area != 3).ToList(), "ShoId", "ShopName");
                if (ModelState.IsValid)
                {
                    if (s.StartDate < s.EndDate)
                    {
                        s.CreatedDate = DateTime.Now;
                        s.ModifiedDate = DateTime.Now;
                        var result = dbContext.Sales.Add(s);
                        dbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("StartDate", "Start must be lesser than StartDate");
                        ModelState.AddModelError("EndDate", "EndDate must be greater than StartDate");
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
            var result = dbContext.Sales.Single(x => x.Sale_id == id);
            ViewBag.Shop_Id = new SelectList(dbContext.Shops.Where(x => x.Service.Service_area != 3).ToList(), "ShoId", "ShopName", result.Shop_ShoId);
            return View(result);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Sale s)
        {
            try
            {
                ViewBag.Shop_Id = new SelectList(dbContext.Shops.Where(x => x.Service.Service_area != 3).ToList(), "ShoId", "ShopName", s.Shop_ShoId);
                ViewBag.Shop_ShoId = new SelectList(dbContext.Shops.ToList(), "ShoId", "ShopName");
                if (ModelState.IsValid)
                {
                    if (s.StartDate < s.EndDate)
                    {
                        Sale sales = dbContext.Sales.Single(x => x.Sale_id == id);
                        sales.Shop_ShoId = s.Shop_ShoId;
                        sales.StartDate = s.StartDate;
                        sales.EndDate = s.EndDate;
                        sales.Sale1 = s.Sale1;
                        sales.slug = s.slug;
                        sales.Meta_title = s.Meta_title;
                        sales.Meta_keyword = s.Meta_keyword;
                        sales.Meta_description = s.Meta_description;
                        sales.ModifiedDate = DateTime.Now;
                        sales.Status = s.Status;
                        dbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("StartDate", "Start must be lesser than StartDate");
                        ModelState.AddModelError("EndDate", "EndDate must be greater than StartDate");
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
            var result = dbContext.Sales.Single(x => x.Sale_id == id);
            ViewBag.Shop_Id = new SelectList(dbContext.Shops.Where(x => x.Service.Service_area != 3).ToList(), "ShoId", "ShopName", result.Shop_ShoId);
            return View(result);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Sale s)
        {
            try
            {
                var result = dbContext.Sales.Single(x => x.Sale_id == id);
                dbContext.Sales.Remove(result);
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