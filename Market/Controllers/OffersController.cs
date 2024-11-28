using Firebase.Auth;
using Firebase.Storage;
using Market.Data.Models;
using Market.Models;
using Market.Services;
using Market.Services.Firebase;
using Market.Services.Offers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;

namespace Market.Controllers
{
    [Authorize]

    public class OffersController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly IUserService _userService;
        private readonly IFirebaseServive _firebaseService;
        private readonly Market.Data.Models.User user;



        public OffersController(IOfferService offerService, IUserService userService, IFirebaseServive firebaseService)
        {
            _offerService = offerService;
            _userService = userService;
            _firebaseService = firebaseService;

            user = _userService.GetUser();
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Offer> offers = await _offerService.GetSellerOffersAsync(user.Id);

            return View(offers);
        }

        [HttpGet]
        public IActionResult AddOffer()
        {
            return View(new AddOfferViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddOffer(AddOfferViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            OfferViewModel offer = model.Offer;
            offer.OwnerId = user.Id;
            await _offerService.AddOfferAsync(user.Id, offer);
            Console.WriteLine("yes");
            List<Offer> offers = await _offerService.GetSellerOffersAsync(offer.OwnerId);
            int id = offers.Last().Id;
            await _firebaseService.UploadFileAsync(model.File, "offers", id.ToString());

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            OfferViewModel offerModel = await _offerService.GetForEditByIdAsync(id);
            IFormFile file = await _firebaseService.GetFileAsync("offers", id.ToString());
            AddOfferViewModel model = new AddOfferViewModel() {File = file, Offer = offerModel};
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddOfferViewModel model)
        {
            await _offerService.EditAsync(model.Offer);
            await _firebaseService.UploadFileAsync(model.File, "offers", model.Offer.Id.ToString());
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _offerService.RemoveOfferAsync(id);
            await _firebaseService.DeleteFileAsync("offers", id.ToString());
            return RedirectToAction("Index");
        }

        [HttpGet] 
        public async Task<IActionResult> Description(int id)
        {
            Offer offer = await _offerService.GetByIdAsync(id);
            ViewBag.url = await _firebaseService.GetImageUrl("offers", id.ToString());
            return View(offer);
        }

        [HttpGet]
        [Authorize(Roles = "Organization")]
        public async Task<IActionResult> Discover()
        {
            List<Offer> offers = await _offerService.GetDiscoverOffers();
            return View(offers);
        }
        
        [HttpPost]
        public IActionResult FilterByCategory(string category, List<Offer> offers)
        {
            var filteredOffers = offers.Where(o => o.Stock.OfferType.Category == category).ToList();
            return View("Discover", filteredOffers);
        }
    }
}
