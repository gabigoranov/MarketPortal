namespace Market.Services.Authentication
{
    public interface IAuthenticationService
    {
        public Task Logout();
        public Task SignInAsync(string userdata, string role);
        public Task SignOutAsync();


    }
}
