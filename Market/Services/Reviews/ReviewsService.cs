using Market.Data.Models;

namespace Market.Services.Reviews
{
    public class ReviewsService : IReviewsService
    {
        private readonly User user;
        private readonly IUserService userService;

        public ReviewsService(IUserService _userService) {
            userService = _userService;
            user = userService.GetUser();

        }


        public List<Review> GetAllReviewsAsync()
        {
            List<Review> reviews = new List<Review>();
            foreach(Offer offer in user.Offers)
            {
                reviews.AddRange(offer.Reviews);
            }
            return reviews;
        }

        public List<Review> GetOfferReviewsAsync(int offerId)
        {
            return user.Offers.Single(x => x.Id == offerId).Reviews.ToList();
        }
    }
}
