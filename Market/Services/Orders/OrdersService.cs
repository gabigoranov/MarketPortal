
using Market.Data.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;
using System.Text.Json;

namespace Market.Services.Orders
{
    public class OrdersService : IOrdersService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient client;

        public OrdersService(IHttpClientFactory httpClientFactory, HttpClient client, IHttpContextAccessor contextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            this.client = client;
            this.client.BaseAddress = new System.Uri("https://farmers-market.somee.com/api/");
            _contextAccessor = contextAccessor;
        }


        public async Task ApproveOrderAsync(int id)
        {
            string url = $"https://farmers-market.somee.com/api/orders/accept?id={id}";
            var response = await client.GetAsync(url); 
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException("Something went wrong with approving the order, please try again");
            }

        }

        public async Task DeclineOrderAsync(int id)
        {
            string url = $"https://farmers-market.somee.com/api/orders/decline?id={id}";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException("Something went wrong with declining the order, please try again");
            }
        }

        public async Task DeliverOrderAsync(int id)
        {
            string url = $"orders/deliver?id={id}";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException("Something went wrong with delivering the order, please try again");
            }
        }

        public async Task<List<Order>> GetUserOrders(Guid id)
        {
            var url = $"https://farmers-market.somee.com/api/orders/getall";
            var response = await client.GetAsync(url);
            var result = new List<Order>();
            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(stringResponse);

                result = JsonSerializer.Deserialize<List<Order>>(stringResponse,
                         new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }
            if (result == null)
            {
                throw new Exception("Error with getting orders");
            }
            return result;
        }
    }
}
