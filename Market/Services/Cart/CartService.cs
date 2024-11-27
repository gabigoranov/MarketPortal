using Market.Data.Models;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Market.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly IHttpClientFactory factory;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly Authentication.IAuthenticationService _authService;
        private readonly HttpClient client;
        private Purchase? _purchase;

        public CartService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, Authentication.IAuthenticationService authService)
        {
            factory = httpClientFactory;
            client = factory.CreateClient();
            client.BaseAddress = new Uri("https://farmers-api.runasp.net/api/");
            _purchase = GetPurchase();
            this.httpContextAccessor = httpContextAccessor;
            _authService = authService;
        }
        public void AddOrder(Order order)
        {
            GetPurchase();
            _purchase!.Orders.Add(order);
            _authService.UpdateCart(_purchase);
            
        }

        public void DeleteOrder(int id)
        {
            GetPurchase();
            _purchase!.Orders.RemoveAt(id);
            _authService.UpdateCart(_purchase);
        }

        public void EditOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public void EmptyCart()
        {
            throw new NotImplementedException();
        }

        public Purchase? GetPurchase()
        {
            try
            {
                var claim = httpContextAccessor?.HttpContext?.User?.Claims?.SingleOrDefault(x => x.Type == "Cart")?.Value;
                if (claim == null)
                {
                    return new Purchase();
                }
                _purchase = JsonConvert.DeserializeObject<Purchase>(claim);
            }
            catch (Exception ex)
            {
                return null;
            }
            return _purchase;
        }

        public async Task Purchase(string address, Guid buyerId)
        {
            GetPurchase();
            _purchase!.Address = address;
            _purchase!.BuyerId = buyerId;
            _purchase!.Price = _purchase!.Orders.Select(x => x.Price).Sum();
            foreach(Order order in _purchase.Orders)
            {
                order.Address = address;
                order.BuyerId = _purchase.BuyerId;
                order.SellerId = order.Offer.OwnerId;
            }
            string url = "https://farmers-api.runasp.net/api/purchases/add/";
            var result = await client.PostAsJsonAsync(url, _purchase);
            _purchase = new Purchase();
            await _authService.UpdateCart(_purchase);

        }

        public void UpdateQuantity(int id, int quantity)
        {
            GetPurchase();
            _purchase!.Orders[id].Quantity = quantity;
            _purchase!.Orders[id].Price = quantity*_purchase!.Orders[id].Offer.PricePerKG;
            _authService.UpdateCart(_purchase);
        }
    }
}
