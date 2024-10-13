namespace Market.Services.Authentication
{
    public interface IAuthenticationService
    {
        public Task SignInAsync(String userdata);
        public Task SignOutAsync();


    }
}
