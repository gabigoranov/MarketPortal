using Market.Data.Models;
using Market.Models;
using Market.Services;
using Market.Services.Firebase;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.Json;
using Market.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
namespace Market.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authService;
        private readonly IFirebaseServive _firebaseService;

        public UserController(IUserService userService, IFirebaseServive firebaseService, IAuthenticationService authService)
        {
            _userService = userService;
            _firebaseService = firebaseService;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            User user = await _userService.Login(model.Email, model.Password);


            string role = "Seller";
            if(user.Discriminator == 2)
            {
                role = "Organization";
                await _authService.SignInAsync(JsonSerializer.Serialize<User>(user), role);
                return RedirectToAction("Discover", "Offers");
            }
            await _authService.SignInAsync(JsonSerializer.Serialize<User>(user), role);
            

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new AddUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.User.Discriminator = 1;
            var statusCode = await _userService.Register(model.User);
            await _firebaseService.UploadFileAsync(model.File, "profiles", model.User.Email);
            
            return RedirectToAction("Login");
        
        }

        [HttpGet]
        public IActionResult RegisterOrganization()
        {
            return View(new AddUserViewModel() { User = new User(){ Discriminator = 2 } });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterOrganization(AddUserViewModel model)
        {

            //TODO: Add validation

            model.User.Discriminator = 2;
            var statusCode = await _userService.Register(model.User);
            await _firebaseService.UploadFileAsync(model.File, "profiles", model.User.Email);

            return RedirectToAction("Login");

        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            User user = _userService.GetUser();
            ViewBag.profilePicture = await _firebaseService.GetImageUrl("profiles", user.Email);
            return View(user);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return RedirectToAction("Landing", "Home");
        }
    }
}
