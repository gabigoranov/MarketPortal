using Market.Data.Models;
using Market.Models;
using Market.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;

namespace Market.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private User _user;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
            _user = _userService.GetUser();
        }

        public IActionResult Index()
        {
            //try to log in with cookies
            string? email = HttpContext.Request.Cookies["Email"];
            string? password = HttpContext.Request.Cookies["Password"];
            System.Diagnostics.Debug.WriteLine(email);

            if (email != null && password != null)
            {
                UserViewModel model = new UserViewModel()
                {
                    Email = email,
                    Password = password,
                };
                return RedirectToAction("Login", "User", model);
            }
            if (_user != null)
            {
                return View();
            }
            return RedirectToAction("Landing");

        }

        public IActionResult Sales()
        {
            System.Diagnostics.Debug.WriteLine(_user.Email);
            return View();
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
