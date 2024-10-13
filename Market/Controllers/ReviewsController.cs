using Market.Data.Models;
using Market.Services;
using Market.Services.Reviews;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewsService _reviewsService;

        public ReviewsController(IReviewsService reviewsService)
        {
            _reviewsService = reviewsService;
        }

        public IActionResult Index()
        {
            return View(_reviewsService.GetAllReviewsAsync());
        }
    }
}
