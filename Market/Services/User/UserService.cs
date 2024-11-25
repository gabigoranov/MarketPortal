using Market.Data.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Diagnostics;
using Market.Services.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Market.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClientFactory factory;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly Authentication.IAuthenticationService authService;
        private readonly HttpClient client;
        private User? User;


        public UserService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, Authentication.IAuthenticationService authService)
        {
            factory = httpClientFactory;
            client = factory.CreateClient();
            client.BaseAddress = new Uri("https://farmers-market.sommee.com/api/");
            User = GetUser();
            this.httpContextAccessor = httpContextAccessor;
            this.authService = authService;
        }

        public async Task<User> Login(string email, string password)
        {
            var url = $"https://farmers-api.runasp.net/api/users/login?email={email}&password={password}";
            var response = await client.GetAsync(url);
            var result = new User();
            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(stringResponse);

                result = JsonSerializer.Deserialize<User>(stringResponse,
                         new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                Debug.WriteLine(response.Content.ToString());
                User = result;
            }
            else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }
            if(result ==  null)
            { 
                throw new Exception("Error with login");
            }
            return result;
        }

        public async Task<HttpStatusCode> Register(User user)
        {
            var url = $"https://farmers-api.runasp.net/api/users/add";
            var jsonParsed = JsonSerializer.Serialize<User>(user, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            HttpContent content = new StringContent(jsonParsed.ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            return response.StatusCode;
        }

        public Task RemoveOrderAsync(int orderId)
        {
            if (User == null)
            {
                throw new Exception("User is not authenticated");
            }
            User.SoldOrders.Remove(User.SoldOrders.Single(x => x.Id == orderId));
            return Task.CompletedTask;

        }
        public Task DeclineOrderAsync(int orderId)
        {
            if(User == null)
            {
                throw new Exception("User is not authenticated");
            }
            if (!User.SoldOrders.Any(x => x.Id == orderId)) throw new Exception("Order does not exist");
            if (User.SoldOrders == null) User.SoldOrders = [];
            User!.SoldOrders!.SingleOrDefault(x => x.Id == orderId)!.IsDenied = true;
            return Task.CompletedTask;
        }

        public async Task AddApprovedOrderAsync(int id)
        {
            var claim = httpContextAccessor?.HttpContext?.User?.Claims?.SingleOrDefault(x => x.Type == ClaimTypes.UserData)?.Value;
            User = JsonSerializer.Deserialize<User>(claim);

            User.SoldOrders.Single(x => x.Id == id).IsAccepted = true;
            string role = "Seller";
            if (User.Discriminator == 2)
            {
                role = "Organization";
            }
            await authService.SignInAsync(JsonSerializer.Serialize<User>(User), role);
        }

        public async Task AddDeliveredOrder(int id)
        {
            var claim = httpContextAccessor?.HttpContext?.User?.Claims?.SingleOrDefault(x => x.Type == ClaimTypes.UserData)?.Value;
            User = JsonSerializer.Deserialize<User>(claim);
            
            User.SoldOrders.Single(x => x.Id == id).IsDelivered = true;
            string role = "Seller";
            if (User.Discriminator == 2)
            {
                role = "Organization";
            }
            await authService.SignInAsync(JsonSerializer.Serialize<User>(User), role);

        }

        public User? GetUser()
        {
            try
            {
                var claim = httpContextAccessor?.HttpContext?.User?.Claims?.SingleOrDefault(x => x.Type == ClaimTypes.UserData)?.Value;
                if (claim == null) return null;
                User = JsonSerializer.Deserialize<User>(claim);
            }
            catch(Exception ex)
            {
                return null;
            }
            return User;
        }
    }
}
