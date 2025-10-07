using Blazored.LocalStorage;
using ETicaret_UI.ViewModals;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Text.Json;


namespace ETicaret_UI.Services
{
    public class CartService
    {
        public class CartItem
        {
            public ProductViewModel Product { get; set; } = null!;
            public int Quantity { get; set; }
        }

        string userId = string.Empty;

        private readonly ILocalStorageService _localStorage;
        private readonly ProtectedSessionStorage _protectedSessionStorage;
        //private const string CartStorageKey = "user_cart";

        private readonly List<CartItem> _items = new();
        public IReadOnlyList<CartItem> Items => _items;

        public event Action? OnChange;

        public CartService(ILocalStorageService localStorage, ProtectedSessionStorage protectedSessionStorage)
        {
            _localStorage = localStorage;
            _protectedSessionStorage = protectedSessionStorage;
        }

        public int TotalCount => _items.Sum(i => i.Quantity);
        public decimal TotalAmount => _items.Sum(i => i.Product.Price * i.Quantity);

        public async Task InitializeAsync(string id)
        {
            userId = id;
            var stored = await _localStorage.GetItemAsStringAsync(GetCartKeyForUser(userId));
            if (!string.IsNullOrEmpty(stored))
            {
                var restored = JsonSerializer.Deserialize<List<CartItem>>(stored);
                if (restored != null)
                {
                    _items.Clear();
                    _items.AddRange(restored);
                    NotifyChanged();
                }
            }
        }

        public async Task AddToCart(ProductViewModel product, int quantity = 1)
        {
            var existing = _items.FirstOrDefault(x => x.Product.Id == product.Id);
            if (existing != null)
                existing.Quantity += quantity;
            else
                _items.Add(new CartItem { Product = product, Quantity = quantity });

            await SaveAsync();
        }

        public async Task RemoveFromCart(int productId)
        {
            var item = _items.FirstOrDefault(x => x.Product.Id == productId);
            if (item != null)
            {
                _items.Remove(item);
                await SaveAsync();
            }
        }

        public async Task UpdateQuantity(int productId, int quantity)
        {
            var item = _items.FirstOrDefault(x => x.Product.Id == productId);
            if (item != null)
            {
                if (quantity <= 0) _items.Remove(item);
                else item.Quantity = quantity;
                await SaveAsync();
            }
        }

        public async Task Clear()
        {
            _items.Clear();
            await _localStorage.RemoveItemAsync(GetCartKeyForUser(userId));
            NotifyChanged();
        }

        private async Task SaveAsync()
        {
            await _localStorage.SetItemAsync(GetCartKeyForUser(userId), _items);
            NotifyChanged();
        }

        private string GetCartKeyForUser(string? userId) =>
    string.IsNullOrEmpty(userId) ? "guest_cart" : $"cart_{userId}";


        private void NotifyChanged() => OnChange?.Invoke();
    }
}