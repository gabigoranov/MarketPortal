using Market.Data.Models;

namespace Market.Services.Cart
{
    public interface ICartService
    {
        public Purchase? GetPurchase();
        public void AddOrder(Order order);
        public void EditOrder(Order order);
        public void DeleteOrder(int id);
        public void EmptyCart();

    }
}
