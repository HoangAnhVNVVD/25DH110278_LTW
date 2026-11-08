using System;
using System.Collections.Generic;
using System.Linq;

namespace _25DH110278_LTW.Models.ViewModel
{
    public class Cart
    {
        private List<CartItem> items = new List<CartItem>();

        public IEnumerable<CartItem> Items => items;

        public void Add(CartItem item)
        {
            var existing = items.FirstOrDefault(i => i.ProductID == item.ProductID);
            if (existing == null)
            {
                items.Add(item);
            }
            else
            {
                existing.Quantity += item.Quantity;
            }
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var existing = items.FirstOrDefault(i => i.ProductID == productId);
            if (existing != null)
            {
                if (quantity > 0)
                {
                    existing.Quantity = quantity;
                }
                else
                {
                    items.Remove(existing);
                }
            }
        }

        public void Remove(int productId)
        {
            items.RemoveAll(i => i.ProductID == productId);
        }

        public void Clear()
        {
            items.Clear();
        }

        public decimal TotalAmount()
        {
            return items.Sum(i => i.TotalPrice);
        }

        public List<IGrouping<string, CartItem>> GroupedItems => items.GroupBy(i => i.Category).ToList();
    }
}