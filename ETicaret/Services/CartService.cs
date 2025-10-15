using Blazored.LocalStorage;
using ETicaret_UI.Settings;
using ETicaret_UI.ViewModals;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Text.Json;

namespace ETicaret_UI.Services
{
    public class CartService
    {
        //public class CartItem
        //{
        //    public ProductViewModel Product { get; set; } = null!;
        //    public int Quantity { get; set; }
        //}

        private string userId = string.Empty;
        private readonly ProtectedSessionStorage _protectedSessionStorage;
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private readonly RequestManager _requestManager;
        private readonly ApiSettings _apiSettings;
        private readonly List<CartItemViewModal> _items = new();

        public IReadOnlyList<CartItemViewModal> Items => _items;
        public event Action? OnChange;

        public CartService(ProtectedSessionStorage protectedSessionStorage
            , ProtectedLocalStorage protectedLocalStorage, RequestManager requestManager, ApiSettings apiSettings)
        {
            _protectedSessionStorage = protectedSessionStorage;
            _requestManager = requestManager;
            _apiSettings = apiSettings;
            _protectedLocalStorage = protectedLocalStorage;
        }


        public int TotalCount => _items.Sum(i => i.Quantity);
        public decimal TotalAmount => _items.Sum(i => i.Product.Price * i.Quantity);

        public async Task InitializeAsync(string id)
        {

            userId = id;
            var cartKey = GetCartKeyForUser(userId);
            var result = await _protectedSessionStorage.GetAsync<List<CartItemViewModal>>(GetCartKeyForUser(userId));

            if (cartKey == "guest_cart")
            {
                result = await _protectedLocalStorage.GetAsync<List<CartItemViewModal>>(GetCartKeyForUser(userId));
            }

            if (result.Success && result.Value != null)
            {
                _items.Clear();
                _items.AddRange(result.Value);
                await GuestCartToUserCart();
                NotifyChanged();
            }
            else if (result.Value == null && userId != null)
            {
                _items.Clear();

                _apiSettings.userId = userId;
                var response = await _requestManager.GetAsync<List<CartItemViewModal>>(_apiSettings.GetCartItemByUserId);
                _apiSettings.userId = string.Empty;
                if (response != null)
                {
                    _items.AddRange(response);
                    await GuestCartToUserCart();
                    await SaveAsync();
                }
            }
        }

        private async Task GuestCartToUserCart()
        {
            var guestCartCheck = await _protectedLocalStorage.GetAsync<List<CartItemViewModal>>(GetCartKeyForUser(string.Empty));
            if (guestCartCheck.Success && guestCartCheck.Value != null)
            {
                foreach (var item in guestCartCheck.Value)
                {
                    await AddToCart(item.Product, item.Quantity);
                }
            }
            await _protectedLocalStorage.DeleteAsync(GetCartKeyForUser(string.Empty));
            await Task.Delay(500);
            await _protectedSessionStorage.DeleteAsync(GetCartKeyForUser(userId));
        }

        public async Task AddToCart(ProductViewModel product, int quantity = 1)
        {
            var existing = _items.FirstOrDefault(x => x.Product?.Id == product.Id);
            if (existing != null)
            {
                existing.Quantity += quantity;
                if (!string.IsNullOrEmpty(userId))
                {
                    _apiSettings.userId = userId;
                    var userCartItems = await _requestManager.GetAsync<List<CartItemViewModal>>(_apiSettings.GetCartItemByUserId);
                    foreach (var item in userCartItems)
                    {
                        if (item.UserId == Convert.ToInt32(userId) && item.ProductId == existing.Product.Id)
                        {
                            _apiSettings.cartItemId = item.Id;
                            var response = await _requestManager.PutAsync<int, bool>(_apiSettings.UpdateCartItem, existing.Quantity);
                            break;
                        }
                    }
                    _apiSettings.userId = string.Empty;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    var item = new CartItemViewModal
                    {
                        ProductId = product.Id,
                        UserId = Convert.ToInt32(userId),
                        Product = product,
                        Quantity = quantity
                    };

                    var response = await _requestManager.PostAsync<CartItemViewModal, bool>(_apiSettings.AddCartItem, item);
                    if (response)
                    {
                        _items.Add(new CartItemViewModal { Product = product, Quantity = quantity });
                    }
                }
                else
                {
                    _items.Add(new CartItemViewModal { Product = product, Quantity = quantity });
                }
            }

            await SaveAsync();
        }

        public async Task RemoveFromCart(int productId)
        {
            var item = _items.FirstOrDefault(x => x.Product.Id == productId);
            if (item != null)
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    _apiSettings.userId = userId;
                    var userCartItems = await _requestManager.GetAsync<List<CartItemViewModal>>(_apiSettings.GetCartItemByUserId);
                    foreach (var item2 in userCartItems)
                    {
                        if (item2.UserId == Convert.ToInt32(userId) && item2.ProductId == item.Product.Id)
                        {
                            _apiSettings.cartItemId = item2.Id;
                            await _requestManager.DeleteAsync(_apiSettings.DeleteCartItem);
                            _items.Remove(item);
                            break;
                        }
                    }
                    _apiSettings.userId = string.Empty;
                }
                else
                {
                    _items.Remove(item);
                }
                await SaveAsync();
            }
        }

        public async Task UpdateQuantity(int productId, int quantity)
        {
            var item = _items.FirstOrDefault(x => x.Product?.Id == productId);
            if (item != null)
            {
                if (quantity <= 0)
                    await RemoveFromCart(productId);
                else
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        _apiSettings.userId = userId;
                        var userCartItems = await _requestManager.GetAsync<List<CartItemViewModal>>(_apiSettings.GetCartItemByUserId);
                        foreach (var item2 in userCartItems)
                        {
                            if (item2.UserId == Convert.ToInt32(userId) && item2.ProductId == item.Product.Id)
                            {
                                _apiSettings.cartItemId = item2.Id;
                                var response = await _requestManager.PutAsync<int, bool>(_apiSettings.UpdateCartItem, quantity);
                                break;
                            }
                        }
                        _apiSettings.userId = string.Empty;
                    }
                    item.Quantity = quantity;
                }
                await SaveAsync();
            }
        }

        public async Task Clear()
        {

            _apiSettings.userId = userId;
            if (userId != null)
            {
                await _requestManager.DeleteAsync(_apiSettings.ClearCart);
                _apiSettings.userId = string.Empty;
            }
            _items.Clear();
            var cartKey = GetCartKeyForUser(userId);
            if (cartKey == "guest_cart")
            {
                await _protectedLocalStorage.DeleteAsync(GetCartKeyForUser(userId));
            }
            else
            {
                await _protectedSessionStorage.DeleteAsync(GetCartKeyForUser(userId));
            }
            NotifyChanged();
        }

        private async Task SaveAsync()
        {
            var cartKey = GetCartKeyForUser(userId);
            if ("guest_cart" == cartKey)
            {
                await _protectedLocalStorage.SetAsync(GetCartKeyForUser(userId), _items);
            }
            else
            {
                await _protectedSessionStorage.SetAsync(GetCartKeyForUser(userId), _items);
            }
            NotifyChanged();
        }

        private string GetCartKeyForUser(string? userId) =>
            string.IsNullOrEmpty(userId) ? "guest_cart" : $"cart_{userId}";

        private void NotifyChanged() => OnChange?.Invoke();
    }
}
