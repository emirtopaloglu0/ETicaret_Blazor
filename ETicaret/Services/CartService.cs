using System;
using System.Collections.Generic;
using System.Linq;
using ETicaret_UI.ViewModals;

namespace ETicaret_UI.Services
{
    public class CartService
    {
        // Basit CartItem - UI'da kullanmak için product referansı saklıyoruz
        public class CartItem
        {
            public ProductViewModel Product { get; set; } = null!;
            public int Quantity { get; set; }
        }

        private readonly List<CartItem> _items = new();
        public IReadOnlyList<CartItem> Items => _items;

        // event ile abonelere haber ver
        public event Action? OnChange;

        public int TotalCount => _items.Sum(i => i.Quantity);

        public decimal TotalAmount => _items.Sum(i => i.Product.Price * i.Quantity);

        public void AddToCart(ProductViewModel product, int quantity = 1)
        {
            var existing = _items.FirstOrDefault(x => x.Product.Id == product.Id);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                _items.Add(new CartItem { Product = product, Quantity = quantity });
            }
            NotifyChanged();
        }

        public void RemoveFromCart(int productId)
        {
            var item = _items.FirstOrDefault(x => x.Product.Id == productId);
            if (item != null)
            {
                _items.Remove(item);
                NotifyChanged();
            }
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var item = _items.FirstOrDefault(x => x.Product.Id == productId);
            if (item != null)
            {
                if (quantity <= 0) _items.Remove(item);
                else item.Quantity = quantity;
                NotifyChanged();
            }
        }

        public void Clear()
        {
            _items.Clear();
            NotifyChanged();
        }

        private void NotifyChanged() => OnChange?.Invoke();
    }
}
