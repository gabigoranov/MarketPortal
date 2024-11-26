using Market.Data.Models;
using Market.Models;
using Market.Services;
using Market.Services.Authentication;
using Market.Services.Inventory;
using Market.Services.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;

namespace Market.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IReviewsService _reviewsService;
        private readonly IAuthenticationService _authService;
        private readonly IInventoryService _inventoryServive;
        private readonly IUserService _userService;
        private User _user;

        public HomeController(ILogger<HomeController> logger, IUserService userService, IAuthenticationService authService, IInventoryService inventoryServive, IReviewsService reviewsService)
        {
            _logger = logger;
            _userService = userService;
            _user = _userService.GetUser();
            _authService = authService;
            _inventoryServive = inventoryServive;
            _reviewsService = reviewsService;
        }

        [Authorize]
        public IActionResult DownloadFile()
        { 
            return Redirect("https://drive.google.com/file/d/1wuDesdIoVtEOSpWBV3y5na3nJxreqGaQ/view?usp=drive_link");
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMinutes(30) } 
            );

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (_user != null)
            {
                if(User.IsInRole("Seller"))
                {
                    List<Stock> stocks = await _inventoryServive.GetSellerStocksAsync();

                    return View(new OverviewViewModel(_user.SoldOrders.ToList(), _reviewsService.GetAllReviewsAsync(), stocks));
                }
                else if (User.IsInRole("Organization"))
                {
                    return RedirectToAction("Discover", "Offers");
                }
                
            }
            return RedirectToAction("Landing");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Help()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Landing()
        {
            return View();
        }
    }
}
