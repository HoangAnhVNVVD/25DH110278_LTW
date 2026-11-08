using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using _25DH110278_LTW.Models;
using _25DH110278_LTW.Models.ViewModel;

namespace _25DH110278_LTW.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        public ActionResult Index(string searchTerm, int? page)
        {
            var model = new HomeProductVM();
            model.SearchTerm = searchTerm;

            var products = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p =>
                    p.ProductName.Contains(searchTerm) ||
                    p.ProductDecription.Contains(searchTerm) ||
                    p.Category.CategoryName.Contains(searchTerm));
            }

            model.FeaturedProducts = db.Products.OrderByDescending(p => p.OrderDetails.Count()).Take(10).ToList();

            int pageNumber = page ?? 1;
            int pageSize = 6;
            model.NewProducts = products.OrderByDescending(p => p.ProductID).ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        public ActionResult ProductDetail(int? id, int? quantity, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var model = new ProductDetailVM();
            model.ProductDetail = product;
            model.Quantity = quantity ?? 1;

            model.RelatedProducts = db.Products
                .Where(p => p.CategoryID == product.CategoryID && p.ProductID != product.ProductID)
                .Take(8)
                .ToList();

            int pageNumber = page ?? 1;
            int pageSize = 3;
            model.TopProducts = db.Products
                .Where(p => p.CategoryID == product.CategoryID)
                .OrderByDescending(p => p.OrderDetails.Count())
                .ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        public ActionResult ProductList(int? categoryId, int? page)
        {
            if (categoryId == null)
            {
                return RedirectToAction("Index");
            }

            var category = db.Categories.Find(categoryId);
            if (category == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryName = category.CategoryName;

            int pageNumber = page ?? 1;
            int pageSize = 6;
            var products = db.Products
                .Where(p => p.CategoryID == categoryId)
                .OrderByDescending(p => p.ProductID)
                .ToPagedList(pageNumber, pageSize);

            return View(products);
        }

        [ChildActionOnly]
        public ActionResult CategoryMenu()
        {
            var categories = db.Categories.ToList();
            return PartialView("CategoryMenuPV", categories);
        }

        [ChildActionOnly]
        public ActionResult PersonalMenu()
        {
            var model = new PersonalMenuVM();
            if (User.Identity.IsAuthenticated)
            {
                model.IsLoggedIn = true;
                model.Username = User.Identity.Name;
            }
            else
            {
                model.IsLoggedIn = false;
            }

            var cart = Session["Cart"] as List<CartItem>;
            model.CartCount = cart != null ? cart.Sum(i => i.Quantity) : 0;

            return PartialView("PersonalMenuPV", model);
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