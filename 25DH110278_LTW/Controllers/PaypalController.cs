using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using _25DH110278_LTW.Models;
using _25DH110278_LTW.Models.ViewModel;
using PayPal.Api;
using Item = PayPal.Api.Item;
using Payer = PayPal.Api.Payer;

namespace _25DH110278_LTW.Controllers
{
    public class PaypalController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        private APIContext GetAPIContext()
        {
            var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            return new APIContext(accessToken);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl, CheckoutVM model)
        {
            var itemList = new ItemList() { items = new List<Item>() };

            foreach (var item in model.CartItems)
            {
                decimal usdPrice = Math.Round(item.UnitPrice / 25000m, 2);
                if (usdPrice <= 0) usdPrice = 1;

                itemList.items.Add(new Item()
                {
                    name = item.ProductName,
                    currency = "USD",
                    price = usdPrice.ToString("0.00"),
                    quantity = item.Quantity.ToString(),
                    sku = item.ProductID.ToString()
                });
            }

            var payer = new Payer() { payment_method = "paypal" };

            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            decimal totalUsd = Math.Round(model.TotalAmount / 25000m, 2);
            if (totalUsd <= 0) totalUsd = 1;

            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = totalUsd.ToString("0.00")
            };

            var amount = new Amount()
            {
                currency = "USD",
                total = totalUsd.ToString("0.00"),
                details = details
            };

            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "Thanh toan don hang MyStore",
                invoice_number = Convert.ToString((new Random()).Next(100000)),
                amount = amount,
                item_list = itemList
            });

            var payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            return payment.Create(apiContext);
        }

        public ActionResult PaymentWithPaypal(CheckoutVM model)
        {
            APIContext apiContext = GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Paypal/PaymentWithPaypal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid, model);

                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    Session.Add(guid, createdPayment.id);
                    Session["PayPalCheckoutModel"] = model;
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }

                    var checkoutModel = Session["PayPalCheckoutModel"] as CheckoutVM;
                    return RedirectToAction("CreateOrderAfterPaypal", "Order", checkoutModel);
                }
            }
            catch (Exception)
            {
                return View("FailureView");
            }
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            var payment = new Payment() { id = paymentId };
            return payment.Execute(apiContext, paymentExecution);
        }

        public ActionResult FailureView()
        {
            return View();
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