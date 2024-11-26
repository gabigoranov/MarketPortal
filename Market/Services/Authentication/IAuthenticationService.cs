using Market.Data.Models;

namespace Market.Services.Authentication
{
    public interface IAuthenticationService
    {
        public Task Logout();
        public Task SignInAsync(string userdata, string role);
        public Task UpdateCart(Purchase purchase);
        public Task UpdateUserData(string userdata);
    }
}
