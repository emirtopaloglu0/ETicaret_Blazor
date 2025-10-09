using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text.Json;
//@inject ProtectedSessionStorage ProtectedSessionStore

namespace ETicaret_UI.Auth
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly ProtectedSessionStorage _protectedSessionStorage;
        private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        public CustomAuthenticationStateProvider(ILocalStorageService localStorage, ProtectedSessionStorage protectedSessionStorage)
        {
            _localStorage = localStorage;
            _protectedSessionStorage = protectedSessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _protectedSessionStorage.GetAsync<string>("authToken");
                if (string.IsNullOrWhiteSpace(token.Value))
                    return new AuthenticationState(_anonymous);

                var claims = JwtParser.ParseClaimsFromJwt(token.Value);
                var identity = new ClaimsIdentity(claims, "jwt");

                // Token süresini kontrol et
                var expClaim = claims.FirstOrDefault(c => c.Type == "exp")?.Value;
                if (expClaim != null && long.TryParse(expClaim, out long expSeconds))
                {
                    var expDate = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;

                    if (expDate < DateTime.UtcNow)
                    {
                        await MarkUserAsLoggedOut();
                        return new AuthenticationState(_anonymous);
                    }
                }

                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            catch
            {
                return new AuthenticationState(_anonymous);
            }
        }

        public async Task MarkUserAsAuthenticated(string token)
        {
            await _protectedSessionStorage.SetAsync("authToken", token);
            var claims = JwtParser.ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _protectedSessionStorage.DeleteAsync("authToken");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }

    public static class JwtParser
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = Convert.FromBase64String(PadBase64(payload));
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
        }

        private static string PadBase64(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return base64;
        }
    }

}
