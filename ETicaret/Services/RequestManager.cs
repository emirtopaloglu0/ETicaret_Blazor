using Blazored.LocalStorage;
using ETicaret_UI.Settings;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;

namespace ETicaret_UI.Services
{
    public class RequestManager
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApiSettings _apiSettings;
        private readonly ILocalStorageService _localStorage;

        public RequestManager(HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor, ApiSettings apiSettings, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _apiSettings = apiSettings;
            _localStorage = localStorage;
        }

        private async Task AddTokenAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", string.Empty);
            }
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            await AddTokenAsync();
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode) return default;

            var json = await response.Content.ReadAsStringAsync();
            // Eğer response tipi string ise JSON parse etme
            if (typeof(T) == typeof(string))
            {
                return (T)(object)json;
            }
            if (string.IsNullOrWhiteSpace(json))
                return default;

            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            await AddTokenAsync();
            var jsonContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, jsonContent);

            if (!response.IsSuccessStatusCode) return default;

            var json = await response.Content.ReadAsStringAsync();

            // Eğer response tipi string ise JSON parse etme
            if (typeof(TResponse) == typeof(string))
            {
                return (TResponse)(object)json;
            }

            if (string.IsNullOrWhiteSpace(json))
                return default;


            return JsonSerializer.Deserialize<TResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            await AddTokenAsync();
            var jsonContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(endpoint, jsonContent);
            //if (!response.IsSuccessStatusCode) return default;
            //response.EnsureSuccessStatusCode();
            //var json = await response.Content.ReadAsStringAsync();
            //return JsonSerializer.Deserialize(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!response.IsSuccessStatusCode) return default;

            var json = await response.Content.ReadAsStringAsync();

            // Eğer response tipi string ise JSON parse etme
            if (typeof(TResponse) == typeof(string))
            {
                return (TResponse)(object)json;
            }

            if (string.IsNullOrWhiteSpace(json))
                return default;


            return JsonSerializer.Deserialize<TResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task DeleteAsync(string endpoint)
        {
            await AddTokenAsync();
            var response = await _httpClient.DeleteAsync(endpoint);
            response.EnsureSuccessStatusCode();
        }
    }
}
