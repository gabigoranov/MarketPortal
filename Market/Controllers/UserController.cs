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

            IFormFile file = await _firebaseService.GetFileAsync("profiles", model.Email);
            using (var inputStream = file.OpenReadStream())
            {
                using (var image = Image.FromStream(inputStream))
                {
                    using (var outputStream = new MemoryStream())
                    {
                        image.Save(outputStream, ImageFormat.Jpeg);

                        System.IO.File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\", "profile.jpg"), outputStream.ToArray());
                    }
                }
            }

            User user = await _userService.Login(model.Email, model.Password);

            await _authService.SignInAsync(JsonSerializer.Serialize(user));
            

            return RedirectToAction("Sales", "Home");
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

            var statusCode = await _userService.Register(model.User);
            await _firebaseService.UploadFileAsync(model.File, "profiles", model.User.Email);
            
            return RedirectToAction("Login");
        
        }

        [HttpGet]
        public IActionResult Profile()
        {
            return View(_userService.GetUser());
        }
    }
}
