using Market.Data.Models;
using System.Net;

namespace Market.Services
{
    public interface IUserService
    {
        public Task<User> Login(string email, string password);
        public Task<HttpStatusCode> Register(User user);
        public Task RemoveOrderAsync(int orderId);
        public Task DeclineOrderAsync(int orderId);
        public User GetUser();
        Task AddApprovedOrderAsync(int id);
        Task AddDeliveredOrder(int id);
    }
}
