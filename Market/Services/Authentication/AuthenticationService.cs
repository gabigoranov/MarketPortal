
using Firebase.Auth;
using Market.Data.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Newtonsoft.Json;

namespace Market.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpClientFactory factory;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly HttpClient client;


        public AuthenticationService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            factory = httpClientFactory;
            client = factory.CreateClient();
            client.BaseAddress = new Uri("https://farmers-api.runasp.net/api/");
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task SignInAsync(string userdata, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.UserData, userdata),
                new Claim(ClaimTypes.Role, role),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.UtcNow,
            };

            await httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public async Task Logout()
        {
            await httpContextAccessor.HttpContext.SignOutAsync();
        }

        public async Task UpdateCart(Purchase purchase)
        {
            var currentClaims = httpContextAccessor.HttpContext.User.Claims.ToList();

            var claimToUpdate = currentClaims.FirstOrDefault(c => c.Type == "Cart");

            if (claimToUpdate != null)
            {
                currentClaims.Remove(claimToUpdate);
                currentClaims.Add(new Claim("Cart", JsonConvert.SerializeObject(purchase)));
            }

            var claimsIdentity = new ClaimsIdentity(
                currentClaims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.UtcNow,
            };

            await httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

        }

        public async Task UpdateUserData(string userdata)
        {
            var currentClaims = httpContextAccessor.HttpContext.User.Claims.ToList();

            var claimToUpdate = currentClaims.FirstOrDefault(c => c.Type == ClaimTypes.UserData);

            if (claimToUpdate != null)
            {
                currentClaims.Remove(claimToUpdate);
                currentClaims.Add(new Claim(ClaimTypes.UserData, userdata));
            }

            var claimsIdentity = new ClaimsIdentity(
                currentClaims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.UtcNow,
            };

            await httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

        }
    }
}
