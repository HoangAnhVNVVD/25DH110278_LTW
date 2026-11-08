using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _25DH110278_LTW.Models.ViewModel
{
    public class CheckoutVM
    {
        public List<CartItem> CartItems { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Địa chỉ nhận hàng không được để trống")]
        [Display(Name = "Địa chỉ nhận hàng")]
        public string ShippingAddress { get; set; }

        public int CustomerID { get; set; }

        public string Username { get; set; }

        public string PaymentMethod { get; set; }

        public string ShippingMethod { get; set; }
    }
}