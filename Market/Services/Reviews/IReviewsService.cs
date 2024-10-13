using Market.Data.Models;

namespace Market.Services.Reviews
{
    public interface IReviewsService
    {
        public List<Review> GetAllReviewsAsync();
        public List<Review> GetOfferReviewsAsync(int offerId);
    }
}
