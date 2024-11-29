using Market.Data.Models;
using Market.Models;
using Market.Services.Authentication;
using System.Text;
using System.Text.Json;

namespace Market.Services.Reviews
{
    public class ReviewsService : IReviewsService
    {
        private User user;
        private readonly IUserService userService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient client;
        private readonly IAuthenticationService _authenticationService;

        public ReviewsService(IUserService _userService, IHttpClientFactory httpClientFactory, IAuthenticationService authenticationService)
        {
            userService = _userService;
            user = userService.GetUser();
            _httpClientFactory = httpClientFactory;
            client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://farmers-api.runasp.net/api/");
            _authenticationService = authenticationService;
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

        public async Task RemoveReviewAsync(int id)
        {
            string url = $"https://farmers-api.runasp.net/api/reviews/delete?id={id}";
            user.Offers.Single(x => x.Reviews.Any(x => x.Id == id)).Reviews.Remove(user.Offers.Single(x => x.Reviews.Any(x => x.Id == id)).Reviews.Single(x => x.Id == id));
            await _authenticationService.UpdateUserData(JsonSerializer.Serialize<User>(user));
            var response = await client.DeleteAsync(url);
        }

        public async Task AddReviewAsync(Review review)
        {
            var jsonParsed = JsonSerializer.Serialize<Review>(review, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            HttpContent content = new StringContent(jsonParsed.ToString(), Encoding.UTF8, "application/json");
            string url = $"https://farmers-api.runasp.net/api/reviews/add/";
            var response = await client.PostAsync(url, content);
        }
    }
}
