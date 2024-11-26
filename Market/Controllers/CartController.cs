using Market.Data.Models;
using Market.Services;
using Market.Services.Cart;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IUserService _userService;
        public CartController(ICartService cartService, IUserService userService)
        {
            _cartService = cartService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Purchase? purchase = _cartService.GetPurchase();
            if (purchase == null) purchase = new Purchase();
            return View(purchase);
        }

        [HttpPost]
        public IActionResult Add(Offer offer, double quantity)
        {
            User user = _userService.GetUser();
            Order order = new Order()
            {
                OfferId = offer.Id,
                Quantity = quantity,
                BuyerId = user.Id,
                SellerId = offer.OwnerId,
                Price = Math.Round(offer.PricePerKG * quantity, 2),
                Title = offer.Title
            };
            _cartService.AddOrder(order);
            return RedirectToAction("Index");
        }
    }
}
