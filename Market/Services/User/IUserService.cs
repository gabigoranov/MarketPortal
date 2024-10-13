using Market.Data.Models;
using System.Net;

namespace Market.Services
{
    public interface IUserService
    {
        public Task<User> Login(string email, string password);
        public Task<HttpStatusCode> Register(User user);
        public Task RemoveOrderAsync(int orderId);
        public User GetUser();
        void AddApprovedOrder(int id);
        void AddDeliveredOrder(int id);
    }
}
