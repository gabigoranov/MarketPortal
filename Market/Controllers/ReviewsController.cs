using Market.Data.Models;
using Market.Models;
using Market.Services;
using Market.Services.Offers;
using Market.Services.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly IReviewsService _reviewsService;
        private readonly IOfferService _offerService;
        private readonly IUserService _userService;

        public ReviewsController(IReviewsService reviewsService, IOfferService offerService, IUserService userService)
        {
            _reviewsService = reviewsService;
            _offerService = offerService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            return View(new ReviewPageViewModel() { Reviews = _reviewsService.GetAllReviewsAsync(), Offers = await _offerService.GetSellerOffersAsync(_userService.GetUser().Id) });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _reviewsService.RemoveReviewAsync(id);
            return RedirectToAction("Index");
        }
    }
}
