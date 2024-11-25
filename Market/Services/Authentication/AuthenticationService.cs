
using Firebase.Auth;
using Market.Data.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

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

        public async Task Logout() => await httpContextAccessor.HttpContext.SignOutAsync();

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

        public Task SignOutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
