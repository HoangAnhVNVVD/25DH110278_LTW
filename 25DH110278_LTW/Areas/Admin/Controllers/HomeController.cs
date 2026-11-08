using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _25DH110278_LTW.Models;
using _25DH110278_LTW.Models.ViewModel;

namespace _25DH110278_LTW.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        public ActionResult Index()
        {
            var stats = db.Products
                .GroupBy(p => p.Category.CategoryName)
                .Select(g => new CategoryStatsVM
                {
                    CategoryName = g.Key,
                    ProductCount = g.Count(),
                    MaxPrice = g.Max(p => p.ProductPrice),
                    MinPrice = g.Min(p => p.ProductPrice),
                    AvgPrice = g.Average(p => p.ProductPrice)
                })
                .ToList();

            return View(stats);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}