
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
            client.BaseAddress = new Uri("https://farmers-market.sommee.com/api/");
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task SignInAsync(string userdata)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.UserData, userdata),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(15),
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
