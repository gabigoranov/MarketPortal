using Market.Data.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Diagnostics;
using Market.Services.Authentication;

namespace Market.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClientFactory factory;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IAuthenticationService authService;
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
            var url = $"https://farmers-market.somee.com/api/users/login?email={email}&password={password}";
            var response = await client.GetAsync(url);
            var result = new User();
            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(stringResponse);

                result = JsonSerializer.Deserialize<User>(stringResponse,
                         new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
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
            var url = $"https://farmers-market.somee.com/api/users/add";
            var jsonParsed = JsonSerializer.Serialize<User>(user, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            HttpContent content = new StringContent(jsonParsed.ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            return response.StatusCode;
        }

        public Task RemoveOrderAsync(int orderId)
        {
            if(User == null)
            {
                throw new Exception("User is not authenticated");
            }
            User.SoldOrders.Remove(User.SoldOrders.Single(x => x.Id == orderId));
            return Task.CompletedTask;
        }

        public void AddApprovedOrder(int id)
        {
            var claim = httpContextAccessor?.HttpContext?.User?.Claims?.SingleOrDefault(x => x.Type == ClaimTypes.UserData)?.Value;
            User = JsonSerializer.Deserialize<User>(claim);

            User.SoldOrders.Single(x => x.Id == id).IsApproved = true;
            authService.SignInAsync(JsonSerializer.Serialize(User));
        }

        public void AddDeliveredOrder(int id)
        {
            var claim = httpContextAccessor?.HttpContext?.User?.Claims?.SingleOrDefault(x => x.Type == ClaimTypes.UserData)?.Value;
            User = JsonSerializer.Deserialize<User>(claim);
            
            User.SoldOrders.Single(x => x.Id == id).IsDelivered = true;
            authService.SignInAsync(JsonSerializer.Serialize(User));

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
