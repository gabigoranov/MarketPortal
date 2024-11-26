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
            client.BaseAddress = new Uri("https://farmers-market.sommee.com/api/");
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
            throw new NotImplementedException();
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
    }
}
