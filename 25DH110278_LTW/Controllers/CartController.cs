using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using _25DH110278_LTW.Models;
using _25DH110278_LTW.Models.ViewModel;

namespace _25DH110278_LTW.Controllers
{
    public class CartController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        private List<CartItem> GetCart()
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public ActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        public ActionResult AddToCart(int id, int? quantity)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                var cart = GetCart();
                var cartItem = cart.FirstOrDefault(i => i.ProductID == id);
                int addQty = quantity ?? 1;

                if (cartItem == null)
                {
                    cart.Add(new CartItem
                    {
                        ProductID = product.ProductID,
                        ProductName = product.ProductName,
                        ProductImage = product.ProductImage,
                        UnitPrice = product.ProductPrice,
                        Quantity = addQty,
                        Category = product.Category.CategoryName
                    });
                }
                else
                {
                    cartItem.Quantity += addQty;
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateQuantity(int id, int quantity)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(i => i.ProductID == id);
            if (cartItem != null)
            {
                if (quantity > 0)
                {
                    cartItem.Quantity = quantity;
                }
                else
                {
                    cart.Remove(cartItem);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(i => i.ProductID == id);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
            }

            return RedirectToAction("Index");
        }

        public ActionResult ClearCart()
        {
            Session["Cart"] = null;
            return RedirectToAction("Index");
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