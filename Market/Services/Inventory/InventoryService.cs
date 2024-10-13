using Market.Data.Models;
using Market.Models;
using System.Text.Json;
using System.Text;
using System.Reflection;

namespace Market.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserService _userService;
        private readonly User user;
        private readonly HttpClient client;

        public InventoryService(IHttpClientFactory httpClientFactory, IUserService userService)
        {
            _httpClientFactory = httpClientFactory;
            client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://farmers-market.sommee.com/api/");
            _userService = userService;
            user = userService.GetUser();
        }
        public async Task AddStockAsync(StockViewModel model)
        {
            model.SellerId = user.Id;

            string url = "https://farmers-market.somee.com/api/stocks/add/";
            var jsonParsed = JsonSerializer.Serialize<StockViewModel>(model, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            HttpContent content = new StringContent(jsonParsed.ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
        }

        public async Task DeleteStockAsync(int id)
        {
            string url = $"https://farmers-market.somee.com/api/stocks/delete?stockId={id}";
            var response = await client.DeleteAsync(url);
        }

        public async Task DownStockAsync(int id, double quantity)
        {
            string url = $"https://farmers-market.somee.com/api/Stocks/down?id={id}&quantity={quantity}";
            var response = await client.GetAsync(url);
        }

        public async Task<List<Stock>> GetSellerStocksAsync()
        {
            string url = $"https://farmers-market.somee.com/api/stocks/get?sellerId={user.Id}";
            List<Stock> response = await client.GetFromJsonAsync<List<Stock>>(url);
            return response;
        }

        public async Task UpStockAsync(ChangeStockViewModel model)
        {
            string url = $"https://farmers-market.somee.com/api/Stocks/up?id={model.Id}&quantity={model.Quantity}";
            var response = await client.GetAsync(url);
        }
    }
}
