using Market.Data.Models;

namespace Market.Services.Reviews
{
    public interface IReviewsService
    {
        public Task AddReviewAsync(Review review);
        public List<Review> GetAllReviewsAsync();
        public List<Review> GetOfferReviewsAsync(int offerId);
        Task RemoveReviewAsync(int id);
    }
}
